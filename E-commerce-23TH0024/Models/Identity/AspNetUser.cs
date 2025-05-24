using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models.Identity;

public partial class AspNetUsers : ApplicationUser
{
    public string Id { get; set; } = null!;


    public override  string? Email { get; set; }

    public override  string UserName { get; set; } = null!;

    public override bool EmailConfirmed { get; set; }

    public override string? PasswordHash { get; set; }

    public override  string? SecurityStamp { get; set; }

    public override string? PhoneNumber { get; set; }

    public override  bool PhoneNumberConfirmed { get; set; }

    public override bool TwoFactorEnabled { get; set; }

    //public DateTime? LockoutEndDateUtc { get; set; }

    public override  bool LockoutEnabled { get; set; }

    public override int AccessFailedCount { get; set; }
    public string Discriminator { get; set; }
    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
