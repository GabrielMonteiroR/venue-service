﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.User;

public class UpdateUserDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [Required]
    [Phone]
    [StringLength(20)]
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
}
