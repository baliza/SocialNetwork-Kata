using NUnit.Framework;
using SocialNetwork_Kata.Core.Models;
using SocialNetwork_Kata.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork_Kata.Core.Tests
{
	[TestFixture]
	public class Test
	{
		[SetUp]
		public void Setup()
		{
		}

		[TearDown]
		public void TearDown()
		{
			IdGenerator.ResetPost();
			IdGenerator.ResetMember();
		}

		[Test]
		public void TotalNumberOfMembersShoudlEqual10()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			Assert.AreEqual(10, social.MemberCount);
		}

		[Test]
		public void AddMemberVerify()
		{
			var social = new SocialNetwork();
			var member = social.AddMember("Jeffrey", "Lambert", "Tulsa", "USA");

			Assert.IsNotNull(member, "should return member");
			Assert.IsNotNull(member.Profile, "Should have profile");

			Assert.AreEqual(1, social.MemberCount, "member count should be 1");
		}

		//[Test]
		//public void VerifyMemberInformation()
		//{
		//	var social = new SocialNetwork();
		//	var data = new TestData(social);

		//	foreach (var m in data.People)
		//	{
		//		var member = m.Value.Member;
		//		var profile = member.Profile;
		//		var person = m.Value.Person;

		//		Assert.AreNotEqual(member.Id, Guid.Empty, "Id incorrect");
		//		Assert.AreEqual(person.Firstname, profile.Firstname, "Firstname incorrect");
		//		Assert.AreEqual(person.Lastname, profile.Lastname, "Lastname incorrect");
		//		Assert.AreEqual(person.City, profile.City, "City incorrect");
		//		Assert.AreEqual(person.Country, profile.Country, "Country incorrect");
		//	}
		//}

		[Test]
		public void FindMemberById()
		{
			var social = new SocialNetwork();
			var member = social.AddMember("Marc", "Andrews", "Sydney", "Australia");
			int id = member.Id;

			Assert.AreEqual(social.FindMemberById(id), member);
		}

		[Test]
		public void FindMemberBySearch()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var member = social.FindMember("Bakken").FirstOrDefault();

			Assert.AreEqual("Filippa", member.Profile.Firstname);
			Assert.AreEqual("Bakken", member.Profile.Lastname);
			Assert.AreEqual("SOMMEN", member.Profile.City);
			Assert.AreEqual("Sweden", member.Profile.Country);
		}

		[Test]
		public void AddFriendRequestShouldBePending()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var m1 = data.GetRandomMember();
			var m2 = data.GetRandomMember(m1.Id);

			// m2 makes request to m1
			m1.AddFriendRequest(m2);

			Assert.AreEqual(0, m1.Friends.Count(), "m1 Should not have friends");
			Assert.AreEqual(1, m1.Pending.Count(), "m1 Should have pending");
		}

		[Test]
		public void RemoveFriendRequestNoPendingOrFriends()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var m1 = data.GetRandomMember();
			var m2 = data.GetRandomMember(m1.Id);

			// m2 makes request to m1
			m1.AddFriendRequest(m2);
			m1.RemoveFriendRequest(m2);

			Assert.AreEqual(0, m1.Friends.Count(), "m1 Should not have friends");
			Assert.AreEqual(0, m1.Pending.Count(), "m1 Should not have pending");
			Assert.AreEqual(0, m2.Friends.Count(), "m2 Should not have friends");
			Assert.AreEqual(0, m2.Pending.Count(), "m2 Should not have pending");
		}

		[Test]
		public void ConfirmFriendRequestShouldBeFriends()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var m1 = data.GetRandomMember();
			var m2 = data.GetRandomMember(m1.Id);

			// m2 makes request to m1
			m1.AddFriendRequest(m2);
			m1.ConfirmFriend(m2);

			Assert.AreEqual(1, m1.Friends.Count(), "m1 Should have friends");
			Assert.AreEqual(0, m1.Pending.Count(), "m1 Should not have pending");
			Assert.AreEqual(1, m2.Friends.Count(), "m2 Should have friends");
			Assert.AreEqual(0, m2.Pending.Count(), "m2 Should not have pending");
		}

		[Test]
		public void ConfirmRequiresFriendRequest()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var m1 = data.GetRandomMember();
			var m2 = data.GetRandomMember(m1.Id);

			m1.ConfirmFriend(m2);

			Assert.AreEqual(0, m1.Friends.Count(), "m1 should not have friends");
			Assert.AreEqual(0, m1.Pending.Count(), "m1 should not have pending");
			Assert.AreEqual(0, m2.Friends.Count(), "m2 should not have friends");
			Assert.AreEqual(0, m2.Pending.Count(), "m2 should not have pending");
		}

		[Test]
		public void CannotAddSameMemberAsFriend()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var m1 = data.GetRandomMember();

			m1.AddFriendRequest(m1);
			m1.ConfirmFriend(m1);

			Assert.AreEqual(0, m1.Friends.Count(), "m1 should not have friends");
			Assert.AreEqual(0, m1.Pending.Count(), "m1 should not have pending");
		}

		[Test]
		public void RemoveFriend()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var m1 = data.GetRandomMember();
			var m2 = data.GetRandomMember(m1.Id);

			// direct friend without confirm
			m1.AddFriendRequest(m2);
			m1.ConfirmFriend(m2);
			m1.RemoveFriend(m2);

			Assert.AreEqual(0, m1.Friends.Count(), "m1 should not have friends");
			Assert.AreEqual(0, m1.Pending.Count(), "m1 should not have pending");
			Assert.AreEqual(0, m2.Friends.Count(), "m2 should not have friends");
			Assert.AreEqual(0, m2.Pending.Count(), "m2 should not have pending");
		}

		[Test]
		public void AddPostsShouldBe2()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var m1 = data.GetRandomMember();

			m1.AddPost(data.GetRandomMessage());
			m1.AddPost(data.GetRandomMessage());

			Assert.AreEqual(2, m1.Posts.Count());
		}

		[Test]
		public void AddRemovePostShouldBe0()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var m1 = data.GetRandomMember();
			var post = m1.AddPost(data.GetRandomMessage());
			m1.RemovePost(post.Id);

			Assert.AreEqual(0, m1.Posts.Count());
		}

		[Test]
		public void VerifyPostInformation()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var m1 = data.GetRandomMember();
			string message = data.GetRandomMessage();

			var post = m1.AddPost(message);

			Assert.AreEqual(1, post.Id, "Post id incorrect");
			Assert.AreEqual(m1, post.Member, "Post member incorrect");
			Assert.That(post.Date, Is.EqualTo(DateTime.Now).Within(5).Seconds, "Post date incorrect");
			Assert.AreEqual(message, post.Message, "Post messages dont match");
		}

		[Test]
		public void LikePostShouldBe5()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);

			var m1 = data.GetRandomMember();
			string message = data.GetRandomMessage();

			var post = m1.AddPost(message);

			post.Likes++;
			post.Likes++;
			post.Likes++;
			post.Likes++;
			post.Likes++;

			Assert.AreEqual(5, post.Likes);
		}

		[Test]
		public void FeedShouldHaveFriendPosts()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);
			var graph = data.CreateSocialGraph(4, new int[] { 0, 2, 3, 3 }, new int[] { 1, 1, 1, 2 });

			var m1 = graph[0]; var m2 = graph[1];
			var m3 = graph[2]; var m4 = graph[3];

			string message1 = data.GetRandomMessage();
			string message2 = data.GetRandomMessage();

			var posts = new List<IPost>() {
				m1.AddPost(message1),
				m2.AddPost(message2),
				m3.AddPost(data.GetRandomMessage()),
				m4.AddPost(data.GetRandomMessage())
			};

			var feedm1 = m1.GetFeed();
			var feedm2 = m2.GetFeed();

			Assert.AreEqual(2, feedm1.Count(), "m1 should have posts");
			Assert.AreEqual(2, feedm1.Count(), "m2 should have posts");

			Assert.AreEqual(1, feedm1.ElementAt(0).Id, "(m1) posts should be ordered by id");
			Assert.AreEqual(2, feedm1.ElementAt(1).Id, "(m1) posts should be ordered by id");
			Assert.AreEqual(1, feedm2.ElementAt(0).Id, "(m2) posts should be ordered by id");
			Assert.AreEqual(2, feedm2.ElementAt(1).Id, "(m2) posts should be ordered by id");

			Assert.AreEqual(message1, feedm1.ElementAt(0).Message, "m1 post message1 does not match");
			Assert.AreEqual(message2, feedm1.ElementAt(1).Message, "m1 post message2 does not match");
			Assert.AreEqual(message1, feedm2.ElementAt(0).Message, "m2 post message1 does not match");
			Assert.AreEqual(message2, feedm2.ElementAt(1).Message, "m2 post message2 does not match");
		}

		[Test]
		public void VerifySocialGraphConnections()
		{
			var data = new TestData(new SocialNetwork());

			// m1 <-> m3, m1 <-> m2, m3 <-> m2, m3 <-> m5, m2 <-> m4, m4 <-> m6, m5 <-> m6
			var graph = data.CreateSocialGraph(6, new int[] { 0, 0, 2, 2, 1, 3, 4 }, new int[] { 2, 1, 1, 4, 3, 5, 5 });

			var m1 = graph[0];
			var m2 = graph[1];
			var m3 = graph[2];
			var m4 = graph[3];
			var m5 = graph[4];
			var m6 = graph[5];

			Assert.IsTrue(m1.Friends.Any(m => m.Id == m3.Id), "m1 <-> m3");
			Assert.IsTrue(m1.Friends.Any(m => m.Id == m2.Id), "m1 <-> m2");
			Assert.IsTrue(m3.Friends.Any(m => m.Id == m2.Id), "m3 <-> m2");
			Assert.IsTrue(m3.Friends.Any(m => m.Id == m5.Id), "m3 <-> m5");
			Assert.IsTrue(m2.Friends.Any(m => m.Id == m4.Id), "m2 <-> m4");
			Assert.IsTrue(m4.Friends.Any(m => m.Id == m6.Id), "m4 <-> m6");
			Assert.IsTrue(m5.Friends.Any(m => m.Id == m6.Id), "m5 <-> m6");

			Assert.IsFalse(m3.Friends.Any(m => m.Id == m4.Id), "m3 should not be connected with m4");
			Assert.IsFalse(m3.Friends.Any(m => m.Id == m6.Id), "m3 should not be connected with m6");
			Assert.IsFalse(m1.Friends.Any(m => m.Id == m4.Id), "m1 should not be connected with m5");
			Assert.IsFalse(m1.Friends.Any(m => m.Id == m5.Id), "m1 should not be connected with m5");
			Assert.IsFalse(m1.Friends.Any(m => m.Id == m6.Id), "m1 should not be connected with m6");
			Assert.IsFalse(m4.Friends.Any(m => m.Id == m5.Id), "m4 should not be connected with m5");
			Assert.IsFalse(m4.Friends.Any(m => m.Id == m3.Id), "m4 should not be connected with m3");
			Assert.IsFalse(m4.Friends.Any(m => m.Id == m1.Id), "m4 should not be connected with m1");
		}

		[Test]
		public void GetFriendsLevel1SameAsFriends()
		{
			var social = new SocialNetwork();
			var data = new TestData(social);
			var graph = data.CreateSocialGraph(3, new int[] { 0, 2, 0 }, new int[] { 1, 1, 2 });

			var m1 = graph[0];
			var m2 = graph[1];
			var m3 = graph[2];

			Assert.AreEqual(m1.GetFriends(1).Count(), m1.Friends.Count(), "m1 friend count incorrect");
			Assert.AreEqual(m2.GetFriends(1).Count(), m2.Friends.Count(), "m2 friend count incorrect");
			Assert.AreEqual(m3.GetFriends(1).Count(), m3.Friends.Count(), "m3 friend count incorrect");
		}

		[Test]
		public void GetFriendsLevel2ShouldReturn2()
		{
			var data = new TestData(new SocialNetwork());
			var graph = data.CreateSocialGraph(3, new int[] { 0, 2, 0 }, new int[] { 1, 1, 2 });
			var m1 = graph[0];
			int actual = m1.GetFriends(2).Count();
			Assert.AreEqual(2, actual);
		}

		[Test]
		public void GetFriendsLevel2ShouldReturn4()
		{
			var data = new TestData(new SocialNetwork());
			var graph = data.CreateSocialGraph(6, new int[] { 0, 0, 2, 2, 1, 3, 4 }, new int[] { 2, 1, 1, 4, 3, 5, 5 });

			Assert.AreEqual(4, graph[0].GetFriends(2).Count());
		}

		[Test]
		public void GetFriendsLevel3ShouldReturn5()
		{
			var data = new TestData(new SocialNetwork());
			var graph = data.CreateSocialGraph(6, new int[] { 0, 0, 2, 2, 1, 3, 4 }, new int[] { 2, 1, 1, 4, 3, 5, 5 });

			Assert.AreEqual(5, graph[0].GetFriends(3).Count());
		}
	}
}