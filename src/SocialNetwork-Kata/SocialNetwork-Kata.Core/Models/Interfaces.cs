using System;
using System.Collections.Generic;

namespace SocialNetwork_Kata.Core.Models
{
	public interface IMember
	{
		IEnumerable<IMember> Friends { get; }
		int Id { get; }
		IEnumerable<IMember> Pending { get; }
		IEnumerable<IPost> Posts { get; }
		IMemberProfile Profile { get; }

		void AddFriendRequest(IMember from);
		IPost AddPost(string message);
		void ConfirmFriend(IMember member);
		IEnumerable<IPost> GetFeed();
		IEnumerable<IMember> GetFriends(int level = 1, IList<int> filter = null);
		void RemoveFriend(IMember member);
		void RemoveFriendRequest(IMember member);
		void RemovePost(int id);
	}
	public interface IMemberProfile
	{
		string City { get; set; }
		string Country { get; set; }
		string Firstname { get; set; }
		string Lastname { get; set; }
		int MemberId { get; set; }
	}
	public interface ISocialNetwork
	{
		int MemberCount { get; }

		IMember AddMember(string firstName, string lastName, string city, string country);
		IEnumerable<IMember> FindMember(string search);
		IMember FindMemberById(int id);
	}
	public interface IPost
	{
		DateTime Date { get; set; }
		int Id { get; set; }
		int Likes { get; set; }
		IMember Member { get; set; }
		string Message { get; set; }
	}
}