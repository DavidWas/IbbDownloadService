using System.ComponentModel.DataAnnotations;

namespace IbbDownloadService.NugetModule.Contracts.DTOs;

public class AddPendingNuget
{
    [Required]
    [StringLength(100, ErrorMessage = "Name is too long.")]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(50, ErrorMessage = "Version is too long.")]
    public string Version { get; set; } = "";

    public string Md5 { get; set; } = "";
}