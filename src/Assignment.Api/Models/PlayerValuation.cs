﻿using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class PlayerValuation
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public decimal ValuationPoints { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Player Player { get; set; }
}