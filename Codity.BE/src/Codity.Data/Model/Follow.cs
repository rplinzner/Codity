﻿namespace Codity.Data.Model
{
    public class Follow : BaseEntity
    {
        public int FollowerId { get; set; }
        public User Follower { get; set; }

        public int FollowingId { get; set; }
        public User Following { get; set; }
    }
}
