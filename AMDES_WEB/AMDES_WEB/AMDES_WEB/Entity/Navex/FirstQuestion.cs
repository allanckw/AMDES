
namespace AMDES_KBS.Entity
{
    public class FirstQuestion
    {
        public static string dataPath = @"\FirstQn.xml";
            //System.Web.HttpContext.Current.Server.MapPath(@"Data\Add\FirstQn.xml");

        private int grpID;

        public int GrpID
        {
            get { return grpID; }
            set { grpID = value; }
        }


    }
}
