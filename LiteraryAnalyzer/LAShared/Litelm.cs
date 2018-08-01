using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiteraryAnalyzer.LAShared {
	public abstract class LitElm {
		public int LitelmID { get; set; }
		public string Header { get; set; }
		public List<LitTag> UserTags { get; set; } = new List<LitTag>();
		public LitTag TreeTag { get; set; } = new LitTag();
		public List<LitEvent> Children { get; set; } = new List<LitEvent>();
	}
	public static partial class LitExtensions {
		public static bool IsElmMergeable(this LitElm elm1, LitElm elm2) {
			if (!elm1.TreeTag.Equals(elm2.TreeTag)) { return false; }
			if (elm1.Children.Count != elm2.Children.Count) { return false; }
			if (elm1.Children.Count == 0 && elm2.Children.Count == 0) { return true; }
			return elm1.Children.Zip(elm2.Children, (c1, c2) => IsElmMergeable(c1, c2)).Aggregate((b1, b2) => b1 && b2);
		}
		public static IEnumerable<LitRef> GetAllReferences(this LitElm elm) {
			var retVal = new List<LitRef>();
			//USES REFLECTION
			var watch1 = elm.GetType().GetProperties()[0];
			var query = elm.GetType().GetProperties().Where(prop => {
				if (prop.PropertyType.IsGenericType) {
					if (prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) {
						var genericTypes = prop.PropertyType.GetGenericArguments();
						if (genericTypes.Count() == 1) {
							return typeof(LitRef).IsAssignableFrom(genericTypes[0]);
						}
					}
				}
				return false;
			});
			foreach (var prop in query) {
				var reflist = prop.GetValue(elm);
				if (reflist != null) {
					var castedList = (reflist as IEnumerable<object>).Cast<LitRef>();
					retVal.AddRange(castedList);
				}
			}
			//REFLECTION OVER
			//Recursive call to the children
			foreach (var child in elm.Children) {
				retVal.AddRange(child.GetAllReferences());
			}
			return retVal.Distinct();
		}
	}
}
