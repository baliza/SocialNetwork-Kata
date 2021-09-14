using SocialNetwork_Kata.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork_Kata.Core.Tests
{
	internal class TestData
	{
		private const int NumberOfMembers = 10;
		private string[,] _predefinedMembers;
		private readonly Random _random;
		private SocialNetwork _social;

		public TestData(SocialNetwork social)
		{
			_social = social;
			_predefinedMembers = new[,]{
{"one", "salva", "zgz", "spain"},
{"two", "la", "zgz", "spain"},
{"three", "chico", "zgz", "spain"},
{"four", "hombre", "zgz", "spain"},
{"five", "chica", "zgz", "spain"},
{"six", "mujer", "zgz", "spain"},
{"seven", "gi", "zgz", "spain"},
{"eigth", "Scrum", "zgz", "spain"},
{"nine", "Scrum", "zgz", "spain"},
{"Filippa", "Bakken", "SOMMEN", "Sweden"},
};
			_random = new Random();
			AddThisManyMembers(NumberOfMembers);
		}

		public IDictionary<int, IMember> People { get; internal set; }
		public IMember[] AddThisManyMembers(int numberOfMembers)
		{
			var members = new List<IMember>();
			for (int i = 0; i < numberOfMembers; i++)
			{
				members.Add(_social.AddMember(_predefinedMembers[i, 0], _predefinedMembers[i, 1], _predefinedMembers[i, 2], _predefinedMembers[i, 3]));
			}
			return members.ToArray();
		}

		public IMember[] CreateSocialGraph(int numberOfMembers, int[] originMember, int[] destinationMember)
		{
			var members = AddThisManyMembers(numberOfMembers); ;
			for (int i = 0; i < originMember.Length; i++)
			{
				var originMemberId = originMember[i];
				var destinationMemberId = destinationMember[i];

				members[originMemberId].AddFriendRequest(members[destinationMemberId]);
				members[originMemberId].ConfirmFriend(members[destinationMemberId]);
			}
			return members.ToArray();
		}

		public int GetRandomId()
		{
			var idx = _random.Next(1, NumberOfMembers);
			Console.WriteLine(idx);
			return idx;
		}

		public IMember GetRandomMember()
		{
			var id = GetRandomId();
			return _social.FindMemberById(id);
		}

		public IMember GetRandomMember(int notThisId)
		{
			var idx = notThisId;
			while (idx == notThisId)
				idx = GetRandomId();
			return _social.FindMemberById(idx);
		}

		public string GetRandomMessage()
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var length = 10;
			var random = new Random();
			var randomString = new string(Enumerable.Repeat(chars, length)
													.Select(s => s[random.Next(s.Length)]).ToArray());
			return randomString;
		}
	}
}