using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NET1705_FService.Repositories.Models;

public partial class Banner
{
    [JsonIgnore]    
    public int Id { get; set; }

    [Required(ErrorMessage = "Banner's Title is required!")]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Banner's Description is required!")]
    [Display(Name = "Description")]
    public string Description { get; set; }
    public string Image { get; set; }

    [Required(ErrorMessage = "Banner's Page is required!")]
    [Display(Name = "Page")]
    public string Page { get; set; }

    [Required(ErrorMessage = "Banner's Status is required!")]
    [Display(Name = "Status")]
    public bool Status { get; set; }
}

