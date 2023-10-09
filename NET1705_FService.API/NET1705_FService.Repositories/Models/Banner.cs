using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

    public partial class Banner
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Page { get; set; }
        public bool Status { get; set; }
    }

