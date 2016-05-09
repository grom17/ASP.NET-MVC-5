namespace ProjectsApp.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProjectExecutors
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int ProjectExecutorId { get; set; }

        public virtual ProjectInfo ProjectInfo { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
