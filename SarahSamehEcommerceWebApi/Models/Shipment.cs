﻿using System.ComponentModel.DataAnnotations.Schema;

namespace SarahSamehEcommerceWebApi.Models;

public class Shipment
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
    public bool IsDeleted { get; set; } = false;
    [ForeignKey("customer")]
    public string CustomerId { get; set; }
    public ApplicationUser Customer { get; set; }

}