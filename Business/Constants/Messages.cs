using Entities.Concrate;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string ProductUpdated = "Ürün güncellendi";
        public static string ProductNotUpdated = "Ürün güncellenirken problem oluştu";
        public static string ProductDeleted = "Ürün silindi";
        internal static string MaintenanceTime = "Sistem bakımda";
        internal static string ProductsListed = "Ürünler listelendi";
        internal static string ProductsMustStartWithA = "Ürünler A harfi ile başlamalı";

        internal static string ProductCountOfCategoryError = "Bir kategoride en fazla 10 ürün olabilir.";
        internal static string ProductNameExist = "Aynı isimde ürün eklenemez.";

        internal static string CategoryLimitExceded = "Kategori sayısı aşıldığı için yeni ürün eklenemez";
        internal static string AuthorizationDenied ="Yetkiniz yoktur.";
    }
}
