using Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(50)]
   
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    
    public string UserPassword { get; set; } = string.Empty;

    [Required, MaxLength(50)]
  
    public string UserName { get; set; } = string.Empty;

    [Range(0, 150)]
    
    public int Age { get; set; }

    [Range(30, 250)]
    
    public int UserHeight { get; set; }

    [Range(20, 300)]
    
    public int UserWeight { get; set; }

    [Range(20, 300)]
    
    public int TargetWeight { get; set; }

    // 🔹 קשר לטבלת העדפות (Preferences) 
     
    public virtual Preferences? Preferences { get; set; } = new Preferences();
}
