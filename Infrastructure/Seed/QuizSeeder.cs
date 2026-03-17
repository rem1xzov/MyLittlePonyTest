using Microsoft.EntityFrameworkCore;
using MyLittlePony_Conexy.Domain;

namespace MyLittlePony_Conexy.Infrastructure.Seed;

public static class QuizSeeder
{
    public static async Task SeedAsync(QuizDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        if (await dbContext.Ponies.AnyAsync(cancellationToken))
        {
            return;
        }

        var ponies = CreatePonies();
        var questions = CreateQuestions(ponies);

        await dbContext.Ponies.AddRangeAsync(ponies, cancellationToken);
        await dbContext.Questions.AddRangeAsync(questions, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static List<Pony> CreatePonies()
    {
        var ponies = new List<Pony>
        {
            new()
            {
                Name = "Twilight Sparkle",
                Description = "Analytical, curious, and deeply reflective leader who values knowledge and friendship.",
                ImageUrl = "https://example.com/twilight-sparkle.png",
                Traits =
                [
                    new PonyTrait { Name = "Analytical" },
                    new PonyTrait { Name = "Introverted" },
                    new PonyTrait { Name = "Perfectionist" },
                    new PonyTrait { Name = "Leader" }
                ]
            },
            new()
            {
                Name = "Rainbow Dash",
                Description = "Bold, competitive, and loyal with a strong need for recognition and speed.",
                ImageUrl = "https://example.com/rainbow-dash.png",
                Traits =
                [
                    new PonyTrait { Name = "Competitive" },
                    new PonyTrait { Name = "Impulsive" },
                    new PonyTrait { Name = "Loyal" },
                    new PonyTrait { Name = "Adventurous" }
                ]
            },
            new()
            {
                Name = "Pinkie Pie",
                Description = "Chaotic good, lives in the moment, always searching for ways to make others smile.",
                ImageUrl = "https://example.com/pinkie-pie.png",
                Traits =
                [
                    new PonyTrait { Name = "Playful" },
                    new PonyTrait { Name = "Spontaneous" },
                    new PonyTrait { Name = "Optimistic" },
                    new PonyTrait { Name = "Energetic" }
                ]
            },
            new()
            {
                Name = "Rarity",
                Description = "Aesthetic, empathetic, and driven by ideals of beauty, harmony, and recognition.",
                ImageUrl = "https://example.com/rarity.png",
                Traits =
                [
                    new PonyTrait { Name = "Perfectionist" },
                    new PonyTrait { Name = "Empathetic" },
                    new PonyTrait { Name = "Creative" },
                    new PonyTrait { Name = "Image-conscious" }
                ]
            },
            new()
            {
                Name = "Applejack",
                Description = "Grounded, honest, and dependable, values hard work and authenticity above all.",
                ImageUrl = "https://example.com/applejack.png",
                Traits =
                [
                    new PonyTrait { Name = "Honest" },
                    new PonyTrait { Name = "Hard-working" },
                    new PonyTrait { Name = "Pragmatic" },
                    new PonyTrait { Name = "Loyal" }
                ]
            },
            new()
            {
                Name = "Fluttershy",
                Description = "Gentle, deeply sensitive, and attuned to others’ emotions, avoids conflict when possible.",
                ImageUrl = "https://example.com/fluttershy.png",
                Traits =
                [
                    new PonyTrait { Name = "Introverted" },
                    new PonyTrait { Name = "Empathetic" },
                    new PonyTrait { Name = "Conflict-avoidant" },
                    new PonyTrait { Name = "Caring" }
                ]
            }
        };

        return ponies;
    }

    private static List<Question> CreateQuestions(List<Pony> ponies)
    {
        Pony Twilight() => ponies.Single(p => p.Name == "Twilight Sparkle");
        Pony Rainbow() => ponies.Single(p => p.Name == "Rainbow Dash");
        Pony Pinkie() => ponies.Single(p => p.Name == "Pinkie Pie");
        Pony Rarity() => ponies.Single(p => p.Name == "Rarity");
        Pony Applejack() => ponies.Single(p => p.Name == "Applejack");
        Pony Fluttershy() => ponies.Single(p => p.Name == "Fluttershy");

        var questions = new List<Question>
        {
            new()
            {
                Text = "Когда ты эмоционально истощён, что помогает тебе восстановиться больше всего?",
                Options =
                [
                    new AnswerOption
                    {
                        Text = "Сесть в тишине, всё разложить по полочкам и понять, что именно пошло не так.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Twilight(), Weight = 4 },
                            new PonyWeight { Pony = Fluttershy(), Weight = 2 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Выплеснуть энергию: спорт, драйв, соревнование или новый вызов.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Rainbow(), Weight = 4 },
                            new PonyWeight { Pony = Applejack(), Weight = 2 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Смеяться с друзьями до слёз, придумать что-то совершенно безумное и весёлое.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Pinkie(), Weight = 4 },
                            new PonyWeight { Pony = Rainbow(), Weight = 2 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Уют, красота вокруг и ощущение, что меня ценят и видят мою уникальность.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Rarity(), Weight = 4 },
                            new PonyWeight { Pony = Fluttershy(), Weight = 2 }
                        ]
                    }
                ]
            },
            new()
            {
                Text = "Как ты обычно принимаешь сложные решения, которые могут изменить твою жизнь?",
                Options =
                [
                    new AnswerOption
                    {
                        Text = "Собираю максимум информации, строю схемы, списки «за» и «против».",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Twilight(), Weight = 5 },
                            new PonyWeight { Pony = Applejack(), Weight = 2 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Доверяю интуиции и ощущениям в моменте – главное, чтобы внутри было ощущение «правильно».",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Rarity(), Weight = 4 },
                            new PonyWeight { Pony = Fluttershy(), Weight = 2 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Сначала действую, потом разбираюсь – жизнь любит смелых.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Rainbow(), Weight = 5 },
                            new PonyWeight { Pony = Pinkie(), Weight = 2 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Смотрю, как моё решение повлияет на близких и на мою ответственность перед ними.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Applejack(), Weight = 4 },
                            new PonyWeight { Pony = Fluttershy(), Weight = 3 }
                        ]
                    }
                ]
            },
            new()
            {
                Text = "Что из этого больше всего похоже на твою скрытую мотивацию, о которой ты редко говоришь вслух?",
                Options =
                [
                    new AnswerOption
                    {
                        Text = "Стать лучшей версией себя и чувствовать, что я контролирую хаос вокруг.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Twilight(), Weight = 4 },
                            new PonyWeight { Pony = Rarity(), Weight = 2 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Доказать себе и миру, что я особенный(ая), смелый(ая) и не такой(ая), как все.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Rainbow(), Weight = 4 },
                            new PonyWeight { Pony = Pinkie(), Weight = 2 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Сделать так, чтобы вокруг было больше тепла, заботы и искренности.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Fluttershy(), Weight = 4 },
                            new PonyWeight { Pony = Applejack(), Weight = 2 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Создать что-то красивое, что вдохновит других и останется после меня.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Rarity(), Weight = 4 },
                            new PonyWeight { Pony = Twilight(), Weight = 2 }
                        ]
                    }
                ]
            },
            new()
            {
                Text = "Как ты реагируешь, когда близкий человек ведёт себя «непо-правилам» и нарушает твои внутренние ожидания?",
                Options =
                [
                    new AnswerOption
                    {
                        Text = "Сначала анализирую, что именно меня задело, а потом стараюсь объяснить логикой и фактами.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Twilight(), Weight = 4 },
                            new PonyWeight { Pony = Applejack(), Weight = 2 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Реагирую резко, могу вспылить, но потом быстро отхожу и делаю вид, что всё ок.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Rainbow(), Weight = 4 },
                            new PonyWeight { Pony = Pinkie(), Weight = 3 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Стараюсь сгладить углы, ухожу в себя, а потом переживаю это долго и молча.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Fluttershy(), Weight = 5 }
                        ]
                    },
                    new AnswerOption
                    {
                        Text = "Становлюсь холоднее, делаю всё правильно, но внутри коплю обиду и ожидание, что меня оценят.",
                        PonyWeights =
                        [
                            new PonyWeight { Pony = Rarity(), Weight = 4 },
                            new PonyWeight { Pony = Twilight(), Weight = 2 }
                        ]
                    }
                ]
            }
            ,
            new()
            {
                Text = "Если бы тебе пришлось выбрать: спасти правду или спасти того, кто не переживёт её, что бы ты сделал(а)?",
                Options =
                [
                    new AnswerOption
                    {
                        Text = "Я бы искал(а) третью формулу: способ сказать правду так, чтобы она не убила, а научила.",
                        PonyWeights = [ new PonyWeight { Pony = Twilight(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Я бы защитил(а) человека: правда без милосердия — просто оружие, и я не хочу держать его в руках.",
                        PonyWeights = [ new PonyWeight { Pony = Fluttershy(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Я бы сказал(а) правду в лицо и взял(а) ответственность за последствия: иначе это трусость, замаскированная заботой.",
                        PonyWeights = [ new PonyWeight { Pony = Applejack(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Я бы сделал(а) так, чтобы правда прозвучала достойно: форма может быть щитом, который не даёт смыслу стать жестокостью.",
                        PonyWeights = [ new PonyWeight { Pony = Rarity(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Я бы выбрал(а) удар: лучше короткая боль сейчас, чем гниль, которая тихо съест всё позже.",
                        PonyWeights = [ new PonyWeight { Pony = Rainbow(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Я бы превратил(а) правду в историю, чтобы её можно было проглотить без крови — пусть смысл дойдёт не сразу, но дойдёт.",
                        PonyWeights = [ new PonyWeight { Pony = Pinkie(), Weight = 5 } ]
                    }
                ]
            },
            new()
            {
                Text = "В момент, когда мир требует от тебя жертвы, что ты отдашь первым — и почему именно это?",
                Options =
                [
                    new AnswerOption
                    {
                        Text = "Собственное спокойствие: пусть внутри будет шторм, если снаружи сохранится порядок, который удержит других от падения.",
                        PonyWeights = [ new PonyWeight { Pony = Twilight(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Своё право на мягкость: иногда я должен(на) стать стеной, даже если ненавижу звук удара по ней.",
                        PonyWeights = [ new PonyWeight { Pony = Fluttershy(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Чужое одобрение: я не торгую совестью, даже если останусь один(одна) на поле, где уже нет аплодисментов.",
                        PonyWeights = [ new PonyWeight { Pony = Applejack(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Иллюзию идеальности: лучше быть несовершенным(ой), чем позволить красивой маске стать причиной чьей-то гибели.",
                        PonyWeights = [ new PonyWeight { Pony = Rarity(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Свою безопасность: если я выживу, но предам свой рывок вперёд, то кто тогда вообще останется живым во мне?",
                        PonyWeights = [ new PonyWeight { Pony = Rainbow(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Свою лёгкость: я могу быть смешным(ой), но могу и стать последним огнём, чтобы другие не замёрзли внутри.",
                        PonyWeights = [ new PonyWeight { Pony = Pinkie(), Weight = 5 } ]
                    }
                ]
            },
            new()
            {
                Text = "Твоя «внутренняя тьма» шепчет тебе в самые слабые минуты. Что она обычно обещает?",
                Options =
                [
                    new AnswerOption
                    {
                        Text = "«Если ты всё просчитаешь, ты больше никогда не потеряешь контроль».",
                        PonyWeights = [ new PonyWeight { Pony = Twilight(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "«Если ты станешь незаметным(ой), тебя не ранят — и ты никому не будешь мешать».",
                        PonyWeights = [ new PonyWeight { Pony = Fluttershy(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "«Если ты будешь прямым(ой) до жестокости, тебя не смогут сломать полуправдами».",
                        PonyWeights = [ new PonyWeight { Pony = Applejack(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "«Если ты будешь безупречным(ой), тебя будут любить — и ты не окажешься лишним(ей)».",
                        PonyWeights = [ new PonyWeight { Pony = Rarity(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "«Если ты всегда впереди, тебя не догонят — и никто не увидит, как ты боишься остановиться».",
                        PonyWeights = [ new PonyWeight { Pony = Rainbow(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "«Если ты заставишь всех улыбаться, никто не спросит, почему ты сам(а) пуст(а) внутри».",
                        PonyWeights = [ new PonyWeight { Pony = Pinkie(), Weight = 5 } ]
                    }
                ]
            },
            new()
            {
                Text = "Представь, что тебе дают власть переписать одно правило реальности. Что ты изменишь, если цена — часть твоей души?",
                Options =
                [
                    new AnswerOption
                    {
                        Text = "Я уберу случайность из катастроф: пусть мир будет сложным, но объяснимым — иначе мы просто бросаем людей в темноту.",
                        PonyWeights = [ new PonyWeight { Pony = Twilight(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Я верну право на тишину: чтобы у каждого было место, где его боль не обязана быть спектаклем.",
                        PonyWeights = [ new PonyWeight { Pony = Fluttershy(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Я сделаю так, чтобы обещания были нерушимыми: слово должно весить больше страха и удобства.",
                        PonyWeights = [ new PonyWeight { Pony = Applejack(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Я подарю миру способность видеть ценность в несовершенном: чтобы сердца перестали ломаться под прессом идеала.",
                        PonyWeights = [ new PonyWeight { Pony = Rarity(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Я сотру грань между «невозможно» и «ещё не пробовали»: иначе люди живут так, будто им уже проиграли заранее.",
                        PonyWeights = [ new PonyWeight { Pony = Rainbow(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Я оставлю людям смех как инстинкт выживания: иногда это единственный мост через бездну, когда логика не работает.",
                        PonyWeights = [ new PonyWeight { Pony = Pinkie(), Weight = 5 } ]
                    }
                ]
            },
            new()
            {
                Text = "Когда тебя загоняют в угол, какой путь спасения кажется тебе самым честным — даже если он пугает?",
                Options =
                [
                    new AnswerOption
                    {
                        Text = "Разобрать ловушку на механизмы: если я понимаю устройство кошмара, я уже наполовину вышел(ла) из него.",
                        PonyWeights = [ new PonyWeight { Pony = Twilight(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Найти того, кого я могу защитить: чужая уязвимость делает меня сильнее, чем собственный страх.",
                        PonyWeights = [ new PonyWeight { Pony = Fluttershy(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Сказать «вот как будет» и сделать: иногда честность — это не слова, а поступок без украшений.",
                        PonyWeights = [ new PonyWeight { Pony = Applejack(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Сохранить достоинство: если я выйду из тьмы униженным(ой), часть меня останется там навсегда.",
                        PonyWeights = [ new PonyWeight { Pony = Rarity(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Пробить стену рывком: даже если я ошибусь, я предпочту падение движению в клетке без попытки.",
                        PonyWeights = [ new PonyWeight { Pony = Rainbow(), Weight = 5 } ]
                    },
                    new AnswerOption
                    {
                        Text = "Зажечь искру вокруг: я не обязан(а) побеждать — иногда достаточно не дать другим окончательно опуститься во мрак.",
                        PonyWeights = [ new PonyWeight { Pony = Pinkie(), Weight = 5 } ]
                    }
                ]
            }
        };

        return questions;
    }
}

