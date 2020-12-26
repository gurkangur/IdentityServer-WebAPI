namespace IdentityServer_WebAPI.Identity.Models
{
    public class ExternalProvider
    {
        public int Id { get; set; }
        public string Fields { get; set; }
        public string Name { get; set; }
        public string UserInfoEndpoint { get; set; }
    }
}
