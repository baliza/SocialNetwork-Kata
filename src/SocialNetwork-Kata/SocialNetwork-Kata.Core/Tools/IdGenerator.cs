using SocialNetwork_Kata.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork_Kata.Core.Tools
{
	public static class IdGenerator
	{
		private static List<int> _memberIds = new List<int>();
		private static List<int> _postIds = new List<int>();

		public static int GetId<T>()
		{
			if (typeof(T) == typeof(IMember))
				return GenerateNextId(_memberIds);
			return GenerateNextId(_postIds);
		}

		public static void ResetMember()
		{
			_memberIds.Clear();
		}

		public static void ResetPost()
		{
			_postIds.Clear();
		}
		private static int GenerateNextId(List<int> idList)
		{
			var id = idList.Any() ? idList.Max(m => m) + 1 : 1;
			idList.Add(id);
			return id;
		}
	}
}