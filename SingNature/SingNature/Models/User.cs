using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [StringLength(100)]
    public string Email { get; set; }

    [Column("password_hash")] // 映射到数据库的 `password_hash` 字段
    public string PasswordHash { get; set; } 

    [Required]
    [StringLength(20)]
    public string Role { get; set; } 

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLogin { get; set; } 
}
