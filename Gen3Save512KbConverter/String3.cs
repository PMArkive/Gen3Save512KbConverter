﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyoutaTools.Pokemon.Gen3 {
    public class String3 {
        public enum Region {
            Japanese,
            Western,
        }

        public Region Language { get; private set; }
        public string Text { get; private set; }
        public byte[] Raw { get; private set; }

        public String3( Region language, int length ) {
            Language = language;
            Raw = new byte[length];
            if ( length > 0 ) {
                Raw[0] = 0xFF;
            }
            for ( int i = 1; i < length; ++i ) {
                Raw[i] = 0;
            }
            Text = "";
        }

        public String3( Region language, System.IO.Stream stream, int length ) {
            Language = language;
            Raw = new byte[length];

            StringBuilder sb = new StringBuilder();
            bool done = false;
            Region currentLanguage = language;
            for ( int i = 0; i < length; ++i ) {
                byte b = (byte)stream.ReadByte();
                Raw[i] = b;
                if ( !done ) {
                    if ( b == 0xFF ) {
                        done = true;
                    } else if ( b < 0xFA ) {
                        switch ( currentLanguage ) {
                            case Region.Japanese:
                                sb.Append( Generation3JapaneseTextLookupTable[b] );
                                break;
                            case Region.Western:
                                sb.Append( Generation3WesternTextLookupTable[b] );
                                break;
                        }
                    } else if ( b == 0xFC ) {
                        byte func = (byte)stream.ReadByte();
                        ++i;
                        Raw[i] = func;
                        switch ( func ) {
                            case 0x15:
                                currentLanguage = Region.Japanese;
                                break;
                            case 0x16:
                                currentLanguage = Region.Western;
                                break;
                            default:
                                Console.WriteLine( "Warning: Unrecognized 0xFC function " + func + " in Gen 3 string, text parsing may be wrong." );
                                break;
                        }
                    } else {
                        Console.WriteLine( "Warning: Unrecognized token " + b + " in Gen 3 string, text parsing may be wrong." );
                    }
                }
            }
            Text = sb.ToString();
        }

        public void Serialize( System.IO.Stream stream ) {
            stream.Write( Raw );
        }

        public override string ToString() {
            return Text;
        }

        public static Dictionary<byte, string> Generation3JapaneseTextLookupTable = new Dictionary<byte, string> {
            {0x00," "},{0x01,"あ"},{0x02,"い"},{0x03,"う"},{0x04,"え"},{0x05,"お"},{0x06,"か"},{0x07,"き"},{0x08,"く"},{0x09,"け"},{0x0A,"こ"},{0x0B,"さ"},{0x0C,"し"},{0x0D,"す"},{0x0E,"せ"},{0x0F,"そ"},
            {0x10,"た"},{0x11,"ち"},{0x12,"つ"},{0x13,"て"},{0x14,"と"},{0x15,"な"},{0x16,"に"},{0x17,"ぬ"},{0x18,"ね"},{0x19,"の"},{0x1A,"は"},{0x1B,"ひ"},{0x1C,"ふ"},{0x1D,"へ"},{0x1E,"ほ"},{0x1F,"ま"},
            {0x20,"み"},{0x21,"む"},{0x22,"め"},{0x23,"も"},{0x24,"や"},{0x25,"ゆ"},{0x26,"よ"},{0x27,"ら"},{0x28,"り"},{0x29,"る"},{0x2A,"れ"},{0x2B,"ろ"},{0x2C,"わ"},{0x2D,"を"},{0x2E,"ん"},{0x2F,"ぁ"},
            {0x30,"ぃ"},{0x31,"ぅ"},{0x32,"ぇ"},{0x33,"ぉ"},{0x34,"ゃ"},{0x35,"ゅ"},{0x36,"ょ"},{0x37,"が"},{0x38,"ぎ"},{0x39,"ぐ"},{0x3A,"げ"},{0x3B,"ご"},{0x3C,"ざ"},{0x3D,"じ"},{0x3E,"ず"},{0x3F,"ぜ"},
            {0x40,"ぞ"},{0x41,"だ"},{0x42,"ぢ"},{0x43,"づ"},{0x44,"で"},{0x45,"ど"},{0x46,"ば"},{0x47,"び"},{0x48,"ぶ"},{0x49,"べ"},{0x4A,"ぼ"},{0x4B,"ぱ"},{0x4C,"ぴ"},{0x4D,"ぷ"},{0x4E,"ぺ"},{0x4F,"ぽ"},
            {0x50,"っ"},{0x51,"ア"},{0x52,"イ"},{0x53,"ウ"},{0x54,"エ"},{0x55,"オ"},{0x56,"カ"},{0x57,"キ"},{0x58,"ク"},{0x59,"ケ"},{0x5A,"コ"},{0x5B,"サ"},{0x5C,"シ"},{0x5D,"ス"},{0x5E,"セ"},{0x5F,"ソ"},
            {0x60,"タ"},{0x61,"チ"},{0x62,"ツ"},{0x63,"テ"},{0x64,"ト"},{0x65,"ナ"},{0x66,"ニ"},{0x67,"ヌ"},{0x68,"ネ"},{0x69,"ノ"},{0x6A,"ハ"},{0x6B,"ヒ"},{0x6C,"フ"},{0x6D,"ヘ"},{0x6E,"ホ"},{0x6F,"マ"},
            {0x70,"ミ"},{0x71,"ム"},{0x72,"メ"},{0x73,"モ"},{0x74,"ヤ"},{0x75,"ユ"},{0x76,"ヨ"},{0x77,"ラ"},{0x78,"リ"},{0x79,"ル"},{0x7A,"レ"},{0x7B,"ロ"},{0x7C,"ワ"},{0x7D,"ヲ"},{0x7E,"ン"},{0x7F,"ァ"},
            {0x80,"ィ"},{0x81,"ゥ"},{0x82,"ェ"},{0x83,"ォ"},{0x84,"ャ"},{0x85,"ュ"},{0x86,"ョ"},{0x87,"ガ"},{0x88,"ギ"},{0x89,"グ"},{0x8A,"ゲ"},{0x8B,"ゴ"},{0x8C,"ザ"},{0x8D,"ジ"},{0x8E,"ズ"},{0x8F,"ゼ"},
            {0x90,"ゾ"},{0x91,"ダ"},{0x92,"ヂ"},{0x93,"ヅ"},{0x94,"デ"},{0x95,"ド"},{0x96,"バ"},{0x97,"ビ"},{0x98,"ブ"},{0x99,"ベ"},{0x9A,"ボ"},{0x9B,"パ"},{0x9C,"ピ"},{0x9D,"プ"},{0x9E,"ペ"},{0x9F,"ポ"},
            {0xA0,"ッ"},{0xA1,"0"},{0xA2,"1"},{0xA3,"2"},{0xA4,"3"},{0xA5,"4"},{0xA6,"5"},{0xA7,"6"},{0xA8,"7"},{0xA9,"8"},{0xAA,"9"},{0xAB,"！"},{0xAC,"？"},{0xAD,"。"},{0xAE,"ー"},{0xAF,"・"},
            {0xB0,"[ ]"},{0xB1,"『"},{0xB2,"』"},{0xB3,"「"},{0xB4,"」"},{0xB5,"♂"},{0xB6,"♀"},{0xB7,"円"},{0xB8,"."},{0xB9,"×"},{0xBA,"/"},{0xBB,"A"},{0xBC,"B"},{0xBD,"C"},{0xBE,"D"},{0xBF,"E"},
            {0xC0,"F"},{0xC1,"G"},{0xC2,"H"},{0xC3,"I"},{0xC4,"J"},{0xC5,"K"},{0xC6,"L"},{0xC7,"M"},{0xC8,"N"},{0xC9,"O"},{0xCA,"P"},{0xCB,"Q"},{0xCC,"R"},{0xCD,"S"},{0xCE,"T"},{0xCF,"U"},
            {0xD0,"V"},{0xD1,"W"},{0xD2,"X"},{0xD3,"Y"},{0xD4,"Z"},{0xD5,"a"},{0xD6,"b"},{0xD7,"c"},{0xD8,"d"},{0xD9,"e"},{0xDA,"f"},{0xDB,"g"},{0xDC,"h"},{0xDD,"i"},{0xDE,"j"},{0xDF,"k"},
            {0xE0,"l"},{0xE1,"m"},{0xE2,"n"},{0xE3,"o"},{0xE4,"p"},{0xE5,"q"},{0xE6,"r"},{0xE7,"s"},{0xE8,"t"},{0xE9,"u"},{0xEA,"v"},{0xEB,"w"},{0xEC,"x"},{0xED,"y"},{0xEE,"z"},{0xEF,"▶"},
            {0xF0,":"},{0xF1,"Ä"},{0xF2,"Ö"},{0xF3,"Ü"},{0xF4,"ä"},{0xF5,"ö"},{0xF6,"ü"},{0xF7,"[⬆]"},{0xF8,"[⬇]"},{0xF9,"[⬅]"},
        };

        public static Dictionary<byte, string> Generation3WesternTextLookupTable = new Dictionary<byte, string> {
            {0x00," "},{0x01,"À"},{0x02,"Á"},{0x03,"Â"},{0x04,"Ç"},{0x05,"È"},{0x06,"É"},{0x07,"Ê"},{0x08,"Ë"},{0x09,"Ì"},{0x0A,"こ"},{0x0B,"Î"},{0x0C,"Ï"},{0x0D,"Ò"},{0x0E,"Ó"},{0x0F,"Ô"},
            {0x10,"Œ"},{0x11,"Ù"},{0x12,"Ú"},{0x13,"Û"},{0x14,"Ñ"},{0x15,"ß"},{0x16,"à"},{0x17,"á"},{0x18,"ね"},{0x19,"ç"},{0x1A,"è"},{0x1B,"é"},{0x1C,"ê"},{0x1D,"ë"},{0x1E,"ì"},{0x1F,"ま"},
            {0x20,"î"},{0x21,"ï"},{0x22,"ò"},{0x23,"ó"},{0x24,"ô"},{0x25,"œ"},{0x26,"ù"},{0x27,"ú"},{0x28,"û"},{0x29,"ñ"},{0x2A,"º"},{0x2B,"ª"},{0x2C,"[ᵉʳ]"},{0x2D,"&"},{0x2E,"+"},{0x2F,"あ"},
            {0x30,"ぃ"},{0x31,"ぅ"},{0x32,"ぇ"},{0x33,"ぉ"},{0x34,"[Lv]"},{0x35,"="},{0x36,";"},{0x37,"が"},{0x38,"ぎ"},{0x39,"ぐ"},{0x3A,"げ"},{0x3B,"ご"},{0x3C,"ざ"},{0x3D,"じ"},{0x3E,"ず"},{0x3F,"ぜ"},
            {0x40,"ぞ"},{0x41,"だ"},{0x42,"ぢ"},{0x43,"づ"},{0x44,"で"},{0x45,"ど"},{0x46,"ば"},{0x47,"び"},{0x48,"ぶ"},{0x49,"べ"},{0x4A,"ぼ"},{0x4B,"ぱ"},{0x4C,"ぴ"},{0x4D,"ぷ"},{0x4E,"ぺ"},{0x4F,"ぽ"},
            {0x50,"っ"},{0x51,"¿"},{0x52,"¡"},{0x53,"[PK]"},{0x54,"[MN]"},{0x55,"[PO]"},{0x56,"[Ké]"},{0x57,"[BL]"},{0x58,"[OC]"},{0x59,"[K]"},{0x5A,"Í"},{0x5B,"%"},{0x5C,"("},{0x5D,")"},{0x5E,"セ"},{0x5F,"ソ"},
            {0x60,"タ"},{0x61,"チ"},{0x62,"ツ"},{0x63,"テ"},{0x64,"ト"},{0x65,"ナ"},{0x66,"ニ"},{0x67,"ヌ"},{0x68,"â"},{0x69,"ノ"},{0x6A,"ハ"},{0x6B,"ヒ"},{0x6C,"フ"},{0x6D,"ヘ"},{0x6E,"ホ"},{0x6F,"í"},
            {0x70,"ミ"},{0x71,"ム"},{0x72,"メ"},{0x73,"モ"},{0x74,"ヤ"},{0x75,"ユ"},{0x76,"ヨ"},{0x77,"ラ"},{0x78,"リ"},{0x79,"⬆"},{0x7A,"⬇"},{0x7B,"⬅"},{0x7C,"➡"},{0x7D,"ヲ"},{0x7E,"ン"},{0x7F,"ァ"},
            {0x80,"ィ"},{0x81,"ゥ"},{0x82,"ェ"},{0x83,"ォ"},{0x84,"ᵉ"},{0x85,"<"},{0x86,">"},{0x87,"ガ"},{0x88,"ギ"},{0x89,"グ"},{0x8A,"ゲ"},{0x8B,"ゴ"},{0x8C,"ザ"},{0x8D,"ジ"},{0x8E,"ズ"},{0x8F,"ゼ"},
            {0x90,"ゾ"},{0x91,"ダ"},{0x92,"ヂ"},{0x93,"ヅ"},{0x94,"デ"},{0x95,"ド"},{0x96,"バ"},{0x97,"ビ"},{0x98,"ブ"},{0x99,"ベ"},{0x9A,"ボ"},{0x9B,"パ"},{0x9C,"ピ"},{0x9D,"プ"},{0x9E,"ペ"},{0x9F,"ポ"},
            {0xA0,"[ʳᵉ]"},{0xA1,"0"},{0xA2,"1"},{0xA3,"2"},{0xA4,"3"},{0xA5,"4"},{0xA6,"5"},{0xA7,"6"},{0xA8,"7"},{0xA9,"8"},{0xAA,"9"},{0xAB,"!"},{0xAC,"?"},{0xAD,"."},{0xAE,"-"},{0xAF,"・"},
            {0xB0,"…"},{0xB1,"“"},{0xB2,"”"},{0xB3,"‘"},{0xB4,"’"},{0xB5,"♂"},{0xB6,"♀"},{0xB7,"$"},{0xB8,","},{0xB9,"×"},{0xBA,"/"},{0xBB,"A"},{0xBC,"B"},{0xBD,"C"},{0xBE,"D"},{0xBF,"E"},
            {0xC0,"F"},{0xC1,"G"},{0xC2,"H"},{0xC3,"I"},{0xC4,"J"},{0xC5,"K"},{0xC6,"L"},{0xC7,"M"},{0xC8,"N"},{0xC9,"O"},{0xCA,"P"},{0xCB,"Q"},{0xCC,"R"},{0xCD,"S"},{0xCE,"T"},{0xCF,"U"},
            {0xD0,"V"},{0xD1,"W"},{0xD2,"X"},{0xD3,"Y"},{0xD4,"Z"},{0xD5,"a"},{0xD6,"b"},{0xD7,"c"},{0xD8,"d"},{0xD9,"e"},{0xDA,"f"},{0xDB,"g"},{0xDC,"h"},{0xDD,"i"},{0xDE,"j"},{0xDF,"k"},
            {0xE0,"l"},{0xE1,"m"},{0xE2,"n"},{0xE3,"o"},{0xE4,"p"},{0xE5,"q"},{0xE6,"r"},{0xE7,"s"},{0xE8,"t"},{0xE9,"u"},{0xEA,"v"},{0xEB,"w"},{0xEC,"x"},{0xED,"y"},{0xEE,"z"},{0xEF,"▶"},
            {0xF0,":"},{0xF1,"Ä"},{0xF2,"Ö"},{0xF3,"Ü"},{0xF4,"ä"},{0xF5,"ö"},{0xF6,"ü"},{0xF7,"[⬆]"},{0xF8,"[⬇]"},{0xF9,"[⬅]"},
        };
    }
}
