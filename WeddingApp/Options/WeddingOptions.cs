namespace WeddingApp.Options;

public class WeddingOptions
{
    public string BrideName { get; set; }
    public string GroomName { get; set; }
    public DateTime WeddingDate { get; set; }
    public long MaxUploadSizeMb { get; set; } = 1024 * 10; // 10GB
    
    public string? UploadDirectory { get; set; }
    
    public bool FileSharingEnabled { get; set; } = true;
    public bool GiftRegistryEnabled { get; set; } = true;
    public bool ReservationsEnabled { get; set; } = true;
    
}

