﻿using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Infrastructure.Data;

namespace OnlineLearningPlatform.Infrastructure.Repositories
{
    public class QuestionRepository(AppDbContext db) : Repository<Question>(db),
        IQuestionRepository
    {
    }
}
