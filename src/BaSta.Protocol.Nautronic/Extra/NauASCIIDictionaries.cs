﻿using System.Collections.Generic;

namespace BaSta.Protocol.Nautronic.Extra;

internal class NauASCIIDictionaries
{
    public static Dictionary<int, char> WesternConversionDictionary = new()
    {
        { 1, 'Й' }, { 2, 'Ч' }, { 3, 'Г' }, { 4, 'Ф' }, { 5, 'Д' }, { 6, 'З' }, { 7, 'Ц' }, { 8, 'Б' }, { 11, 'Ш' }, { 12, 'Щ' }, { 14, 'У' }, { 15, 'И' }, { 16, 'П' }, { 17, 'Ю' }, { 18, 'Э' }, { 19, 'Ж' }, { 20, 'Ъ' }, { 21, 'Л' }, { 22, 'К' }, { 23, 'Ğ' }, { 24, 'İ' }, { 25, 'Ş' }, { 26, 'Ē' }, { 65, 'А' }, { 66, 'В' }, { 67, 'С' }, { 69, 'Е' }, { 72, 'Н' }, { 77, 'М' }, { 79, 'О' }, { 80, 'Р' }, { 84, 'Т' }, { 88, 'Х' }, { 103, 'Я' }, { 104, 'Ы' }, { 105, 'Ь' }, { 123, 'Ā' }, { 125, 'Ī' }, { 126, '~' }, { sbyte.MaxValue, 'Ō' }, { 128, '€' }, { 129, 'Ą' }, { 130, 'Ć' }, { 131, 'Ę' }, { 132, 'Ł' }, { 133, 'Ń' }, { 134, 'Ś' }, { 135, 'Ź' }, { 136, 'Ż' }, { 137, 'Ū' }, { 138, 'Š' }, { 139, 'Ģ' }, { 140, 'Œ' }, { 141, 'Ķ' }, { 142, 'Ž' }, { 143, 'Ņ' }, { 144, 'Ŗ' }, { 145, 'Č' }, { 146, 'Ļ' }, { 153, '\u0099' }, { 159, 'Ÿ' }, { 169, '©' }, { 174, '®' }, { 192, 'À' }, { 193, 'Á' }, { 194, 'Â' }, { 195, 'Ã' }, { 196, 'Ä' }, { 197, 'Å' }, { 198, 'Æ' }, { 199, 'Ç' }, { 200, 'È' }, { 201, 'É' }, { 202, 'Ê' }, { 203, 'Ë' }, { 204, 'Ё' }, { 205, 'Í' }, { 206, 'Î' }, { 207, 'Ï' }, { 208, 'Ð' }, { 209, 'Ñ' }, { 211, 'Ó' }, { 212, 'Ô' }, { 213, 'Õ' }, { 214, 'Ö' }, { 216, 'Ø' }, { 217, 'Ù' }, { 218, 'Ú' }, { 219, 'Û' }, { 220, 'Ü' }, { 221, 'Ý' }, { 222, 'Þ' }, { 223, 'ß' }, { 224, 'ש' }, { 225, 'נ' }, { 226, 'ב' }, { 227, 'ג' }, { 228, 'ק' }, { 229, 'כ' }, { 230, 'ע' }, { 231, 'י' }, { 232, 'ן' }, { 233, 'ח' }, { 234, 'ל' }, { 235, 'ך' }, { 236, 'צ' }, { 237, 'מ' }, { 238, 'ם' }, { 239, 'פ' }, { 241, 'ר' }, { 242, 'ד' }, { 243, 'א' }, { 244, 'ו' }, { 245, 'ה' }, { 246, 'י' }, { 247, 'ס' }, { 248, 'ט' }, { 249, 'ז' }, { 251, 'ף' }, { 253, 'ץ' }, { 254, 'ת' }
    };

    public static Dictionary<int, char> ArabicConversionDictionary = new()
    {
        { 1, 'Й' }, { 2, 'Ч' }, { 3, 'Г' }, { 4, 'Ф' }, { 5, 'Д' }, { 6, 'З' }, { 7, 'Ц' }, { 8, 'Б' }, { 11, 'Ш' }, { 12, 'Щ' }, { 14, 'У' }, { 15, 'И' }, { 16, 'П' }, { 17, 'Ю' }, { 18, 'Э' }, { 19, 'Ж' }, { 20, 'Ъ' }, { 21, 'Л' }, { 22, 'К' }, { 23, 'Ğ' }, { 24, 'İ' }, { 25, 'Ş' }, { 26, 'Ē' }, { 123, 'Ā' }, { 124, '|' }, { 125, 'Ī' }, { 126, '~' }, { sbyte.MaxValue, 'Ō' }, { 128, '€' }, { 129, 'Ą' }, { 130, 'Ć' }, { 131, 'Ę' }, { 132, 'Ł' }, { 133, 'Ń' }, { 134, 'Ś' }, { 135, 'Ź' }, { 136, 'Ż' }, { 137, 'Ū' }, { 138, 'Š' }, { 139, 'Ģ' }, { 140, 'Œ' }, { 141, 'Ķ' }, { 142, 'Ž' }, { 143, 'Ņ' }, { 144, 'Ŗ' }, { 145, 'Č' }, { 146, 'Ļ' }, { 153, '\u0099' }, { 155, 'ص' }, { 156, char.MaxValue }, { 157, 'ض' }, { 158, char.MaxValue }, { 159, 'Ÿ' }, { 161, 'و' }, { 162, 'ي' }, { 163, '£' }, { 164, 'ا' }, { 165, 'ب' }, { 166, 'ت' }, { 167, 'ث' }, { 168, 'ج' }, { 169, '©' }, { 170, 'ح' }, { 171, 'خ' }, { 172, 'د' }, { 173, 'ذ' }, { 174, '®' }, { 175, 'ر' }, { 176, 'ز' }, { 177, 'س' }, { 178, char.MaxValue }, { 179, 'ش' }, { 180, char.MaxValue }, { 181, 'ط' }, { 182, 'ظ' }, { 183, 'ع' }, { 184, 'غ' }, { 185, 'ف' }, { 186, 'ق' }, { 187, 'ك' }, { 188, 'ل' }, { 189, 'م' }, { 190, 'ن' }, { 191, 'ه' }, { 192, 'À' }, { 193, 'Á' }, { 194, 'Â' }, { 195, 'Ã' }, { 196, 'Ä' }, { 197, 'Å' }, { 198, 'Æ' }, { 199, 'Ç' }, { 200, 'È' }, { 201, 'É' }, { 202, 'Ê' }, { 203, 'Ë' }, { 205, 'Í' }, { 206, 'Î' }, { 207, 'Ï' }, { 208, 'Ð' }, { 209, 'Ñ' }, { 211, 'Ó' }, { 212, 'Ô' }, { 213, 'Õ' }, { 214, 'Ö' }, { 216, 'Ø' }, { 217, 'Ù' }, { 218, 'Ú' }, { 219, 'Û' }, { 220, 'Ü' }, { 221, 'Ý' }, { 222, 'Þ' }, { 223, 'ß' }, { 224, 'ى' }, { 225, 'ـ' }, { 226, 'ئ' }, { 227, 'ء' }, { 228, 'ؤ' }, { 232, 'ة' }, { 233, 'ؚ' }, { 234, 'ً' }, { 235, 'ؙ' }, { 237, 'ؘ' }, { 239, 'ٲ' }, { 240, 'أ' }, { 241, 'ٳ' }, { 242, 'ڲ' }, { 243, 'ە' }, { 244, '؟' }, { 245, 'أ' }, { 246, 'ّ' }, { 247, 'ْ' }, { 249, 'ٍ' }, { 250, 'ٺ' }, { 251, '،' }, { 252, 'ی' }, { 253, 'إ' }, { 254, 'آ' }
    };
}