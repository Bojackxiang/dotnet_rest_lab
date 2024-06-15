using RestApi.Models;

namespace RestApi.data;

public class VillaStore
{
    public static List<VillaDTO> villaStore()
    {
        var v1 = new VillaDTO { Id = "1", Name = "v1" };
        var v2 = new VillaDTO { Id = "2", Name = "v2" };
        var v3 = new VillaDTO { Id = "3", Name = "v3" };

        var villaStore = new List<VillaDTO>
        {
            v1, v2, v3
        };


        return villaStore;
    }
}