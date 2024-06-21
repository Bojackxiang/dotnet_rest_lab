using RestApi.Models;

namespace RestApi.data;

public class VillaStore
{
    public static List<VillaDTO> villaStore()
    {
        var v1 = new VillaDTO { Id = "1", Name = "v1", Age = 12 };
        var v2 = new VillaDTO { Id = "2", Name = "v2", Age = 23 };
        var v3 = new VillaDTO { Id = "3", Name = "v3", Age = 33 };

        var villaStore = new List<VillaDTO>
        {
            v1, v2, v3
        };


        return villaStore;
    }
}