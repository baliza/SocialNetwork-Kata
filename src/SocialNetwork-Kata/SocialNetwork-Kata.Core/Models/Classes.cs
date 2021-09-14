using SocialNetwork_Kata.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork_Kata.Core.Models
{
	public class SocialNetwork : ISocialNetwork
	{
		private List<IMember> _members = new List<IMember>();

		// Adds a new member and returns the added member
		public IMember AddMember(string firstName, string lastName, string city, string country)
		{
			//var id = _members.Any() ? _members.Max(m => m.Id) : 0;

			var id = IdGenerator.GetId<IMember>();

			var member = Member.Create(id, firstName, lastName, city, country);
			_members.Add(member);
			return member;
		}

		// Returns the member with the id
		public IMember FindMemberById(int id)
		{
			return _members.FirstOrDefault(m => m.Id == id);
		}

		// Returns a list of members by searching all fields in the profile
		public IEnumerable<IMember> FindMember(string search)
		{
			return _members.Where(m => AnyFieldLike(m.Profile, search));
		}

		public bool AnyFieldLike(IMemberProfile memberProfile, string search)
		{
			return memberProfile.Firstname == search || memberProfile.Lastname == search || memberProfile.City == search || memberProfile.Country == search;
		}

		// Total number of members currently in the social network
		public int MemberCount { get { return _members.Count; } }
	}

	public class Member : IMember
	{
		private List<IMember> _friends;
		private List<IMember> _pending;
		private List<IPost> _feed;

		private Member(int id, MemberProfile profile)
		{
			Id = id;
			Profile = profile;
			_friends = new List<IMember>();
			_pending = new List<IMember>();
			_feed = new List<IPost>();
		}

		// Id of member. Must be unique and sequential.
		public int Id { get; }

		// Member profile
		public IMemberProfile Profile { get; }

		// List of friends
		public IEnumerable<IMember> Friends { get { return _friends; } }

		// List of pending friend requests
		public IEnumerable<IMember> Pending { get { return _pending; } }

		// Members posts
		public IEnumerable<IPost> Posts { get { return _feed; } }

		// Adds a friend request for this member. from - the member making the friend request
		public void AddFriendRequest(IMember from)
		{
			if (from.Id == this.Id)
				return;
			if (_pending.Any(f => f.Id == from.Id))
				return;
			_pending.Add(from);
			from.AddFriendRequest(this);
		}

		// Confirms a pending friend request
		public void ConfirmFriend(IMember member)
		{
			var pendingFriend = _pending.FirstOrDefault(m => m.Id == member.Id);
			if (pendingFriend == null)
				return;
			if (member.Id == Id)
				return;

			_pending.Remove(pendingFriend);
			if (_friends.Any(f => f.Id == member.Id))
				return;

			_friends.Add(pendingFriend);
			pendingFriend.ConfirmFriend(this);
		}

		// Removes a pending friend request
		public void RemoveFriendRequest(IMember member)
		{
			var pendingFriend = _pending.FirstOrDefault(m => m.Id == member.Id);
			if (pendingFriend == null)
				return;
			_pending.Remove(pendingFriend);
			pendingFriend.RemoveFriendRequest(this);
		}

		// Removes a friend
		public void RemoveFriend(IMember member)
		{
			var friend = _friends.FirstOrDefault(m => m.Id == member.Id);
			if (friend == null)
				return;
			_friends.Remove(friend);
			friend.RemoveFriend(this);
		}

		// Returns a list of all friends. level - depth (1 - friends, 2 - friends of friends ...)
		public IEnumerable<IMember> GetFriends(int level = 1, IList<int> filter = null)
		{
			if (filter == null)
				filter = new List<int>();
			filter.Add(Id);
			var result = new List<IMember>();
			foreach (var f in _friends.Where(f => !filter.Contains(f.Id)))
			{
				result.Add(f);
				if (level > 1)
				{
					var friendsOfMyFriends = f.GetFriends(level - 1, filter);
					result.AddRange(friendsOfMyFriends);
				}
			}
			var distinctItems = result.GroupBy(x => x.Id).Select(y => y.First());
			List<IMember> members = distinctItems.ToList();
			return members;
		}

		// Adds a new post to members feed
		public IPost AddPost(string message)
		{
			//var postId = _feed.Any() ? _feed.Max(m => m.Id) : 0;
			var postId = IdGenerator.GetId<IPost>();

			var post = new Post
			{
				Id = postId,
				Date = DateTime.Now,
				Member = this,
				Message = message
			};

			_feed.Add(post);
			return post;
		}

		// Removes the post with the id
		public void RemovePost(int id)
		{
			var post = _feed.FirstOrDefault(p => p.Id == id);
			if (post == null)
				return;
			_feed.Remove(post);
		}

		// Returns members feed as a list of posts. Should also return posts of friends.
		public IEnumerable<IPost> GetFeed()
		{
			var posts = new List<IPost>();
			posts.AddRange(_feed);

			foreach (var f in _friends)
			{
				posts.AddRange(f.Posts);
			}

			return posts.OrderBy(p => p.Id).ToList();
		}

		internal static IMember Create(int id, string firstName, string lastName, string city, string country)
		{
			return new Member(id, new MemberProfile
			{
				MemberId = id,
				Firstname = firstName,
				Lastname = lastName,
				City = city,
				Country = country
			});
		}
	}

	public class MemberProfile : IMemberProfile
	{
		// Id of the Member this profile belongs to
		public int MemberId { get; set; }

		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
	}

	public class Post : IPost
	{
		// Id of post. Must be unique and sequential.
		public int Id { get; set; }

		// Member that made this post
		public IMember Member { get; set; }

		// The post message
		public string Message { get; set; }

		// Date and time post was made
		public DateTime Date { get; set; }

		// Likes for post
		public int Likes { get; set; }
	}
}