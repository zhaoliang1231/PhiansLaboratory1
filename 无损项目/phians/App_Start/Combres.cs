[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(phians.App_Start.Combres), "PreStart")]
namespace phians.App_Start {
	using System.Web.Routing;
	using global::Combres;
	
    public static class Combres {
        public static void PreStart() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}