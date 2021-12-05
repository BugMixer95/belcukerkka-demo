namespace Belcukerkka.PdfGenerator
{
    /// <summary>
    /// Default values used in documents creation.
    /// </summary>
    public static class DocumentDefaults
    {
        #region Dimensions Defaults
        public static int A4Width { get; } = 1240; // 21cm, PPI = 150
        public static int A4Height { get; } = 1754; // 29.7cm, PPI = 150
        public static double Offset { get; } = 50d;
        #endregion

        #region Font Defaults
        public static string FontFamilyArial { get; } = "Arial";
        public static string FontFamilyTimesNewRoman { get; } = "Times New Roman";
        public static int FontSizeRegular { get; } = 24;
        public static int FontSizeTableContent { get; } = 20;
        public static int FontSizeHeaderSmall { get; } = 30;
        public static int FontSizeHeaderLarge { get; } = 36;
        #endregion

        #region Legal Entity Defaults
        public static string LegalEntityBelcukerkka { get; } = "ООО \"БЕЛЦУКЕРКА\"" +
            "\nРеспублика Беларусь, 220103, г.Минск, ул.Калиновского, д.55," +
            "\nкомната 45Т" +
            "\nУНП: 193463647" +
            "\nр/с: BY36ALFA30122686700010270000" +
            "\nв ЗАО 'Альфа-Банк'," +
            "\nБИК: ALFABY2X";
        public static string LegalEntityOriolTrade { get; } = "ООО \"ОРИОЛ ТРЕЙД\"" +
            "\nРеспублика Беларусь, 220035, г.Минск, ул.Тимирязева, д.65А/401" +
            "\nУНП: 193588530" +
            "\nр/с: BY35PJCB30120681281000000933" +
            "\nв ОАО 'Приорбанк'," +
            "\nБИК: PJCBBY2X" +
            "\nАдрес банка: г.Минск, ул.Веры Хоружей, д.31А";
        #endregion

        #region Files Defaults
        public static string ImagePath { get; } = "pdf-docs\\images";
        public static string TemplatePath { get; } = "pdf-docs\\templates";
        #endregion
    }
}
