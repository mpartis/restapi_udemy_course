using System.Net;

namespace MagicVilla_VillaAPI.Models
{
    public class APIResponse
    {
        // kserw gw apo edw kai pera ola ta endpoints tha epistrefoun
        // APIResponse kai to ekastote villa, villadto ktl tha mpainei sto
        // object result.
        // Etsi ola ta endpoints tha epistrefoun to idio type
        // Kathws etsi prepei sta sovara apis symfwna me ton indo daskalo
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
