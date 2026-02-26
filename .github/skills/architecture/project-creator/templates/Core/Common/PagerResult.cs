using Sanjel.eServiceCloud.Core.Models;

namespace Sanjel.eServiceCloud.Core.Common
{
	public class PagerResult<T> where T : IModel
    {
        public PagerResult()
        {
            this.Result = new List<T>();
            this.Pager = new Pager();
        }

        public List<T> Result { get; set; }

        public Pager Pager { get; set; }
    }
}
