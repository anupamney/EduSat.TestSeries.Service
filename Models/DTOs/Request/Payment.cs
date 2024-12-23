﻿namespace EduSat.TestSeries.Service.Models.DTOs.Request
{
    public class Payment
    {
        public int Id { get; set; }
        public int ScholarshipId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public bool PaymentStatus { get; set; }
        public int DiscountPercent { get; set; }
        public decimal DiscountedPrice { get; set; }
    }
}
