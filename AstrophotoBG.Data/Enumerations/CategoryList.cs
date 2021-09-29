using System.ComponentModel;

namespace AstrophotoBG.Data.Enumerations
{
    public enum CategoryList
    {
        [Description("galaxies")]
        galaxies = 0,

        [Description("planets")]
        planets = 1,

        [Description("star clusters")]
        starClusters = 2,

        [Description("nebulas")]
        nebulas = 3,

        [Description("sun")]
        sun = 4, 

        [Description("moon")]
        moon = 5,

        [Description("milky way")]
        milkyWay = 6,

        [Description("stars")]
        stars = 7,

        [Description("others")]
        others = 8
    }
}
