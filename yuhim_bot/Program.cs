using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using Telegram.Bot.Requests;

class Program
{
    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private static ITelegramBotClient _botClient;

    // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
    private static ReceiverOptions _receiverOptions;

    static async Task Main()
    {

        _botClient = new TelegramBotClient("6690163033:AAGssesfGAb8Hc4HbgMNM-7uBCmrjDONtdI"); // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather
        _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
        {
            AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
            {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                UpdateType.CallbackQuery // Inline кнопки
            },
            // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
            // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
            ThrowPendingUpdates = true,
        };

        using var cts = new CancellationTokenSource();

        // UpdateHander - обработчик приходящих Update`ов
        // ErrorHandler - обработчик ошибок, связанных с Bot API
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота UpdateType

        var me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
        Console.WriteLine($"{me.FirstName} запущен!");

        await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
    }

    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
        try
        {
            // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        // эта переменная будет содержать в себе все связанное с сообщениями
                        var message = update.Message;

                        // From - это от кого пришло сообщение
                        var user = message.From;

                        // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                        Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                        // Chat - содержит всю информацию о чате
                        var chat = message.Chat;

                        // Добавляем проверку на тип Message
                        switch (message.Type)
                        {
                            // Тут понятно, текстовый тип
                            case MessageType.Text:
                                {
                                    // тут обрабатываем команду /start, остальные аналогично
                                    if (message.Text == "/start")
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Выбери клавиатуру:\n" +
                                            "/inline\n" +
                                            "/reply\n");
                                        return;
                                    }
                                    if (string.Equals(message.Text, "привет", StringComparison.OrdinalIgnoreCase) || string.Equals(message.Text, "привет!", StringComparison.OrdinalIgnoreCase))
                                    {
                                                                             
                                        string senderLastName = chat.LastName;
                                        string senderFirstName = chat.FirstName;

                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            $"Привет {senderFirstName} {senderLastName}!"
                                            );
                                        return;
                                    }
                                    if (string.Equals(message.Text, "пока", StringComparison.OrdinalIgnoreCase) || string.Equals(message.Text, "пока!", StringComparison.OrdinalIgnoreCase))
                                    {

                                        string senderLastName = chat.LastName;
                                        string senderFirstName = chat.FirstName;

                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            $"Прощай {senderFirstName} {senderLastName}!"
                                            );
                                        return;
                                    }
                                    if (string.Equals(message.Text, "позвони мне", StringComparison.OrdinalIgnoreCase) || string.Equals(message.Text, "позвони мне!", StringComparison.OrdinalIgnoreCase))
                                    {
                                       
                                            await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Я не могу, у меня клешни :("
                                            );
                                        return;
                                    }
                                    if (string.Equals(message.Text, "напиши моему соседу", StringComparison.OrdinalIgnoreCase) || string.Equals(message.Text, "напиши моему соседу!", StringComparison.OrdinalIgnoreCase))
                                    {

                                        List<long> receiverChatIds = new List<long>      // Получите список chat.Id для 10 пользователей
                                                {
                                                     940442735,  // chat.Id первого пользователя
                                                     987654321,  // chat.Id второго пользователя
                                                     656730512,  // Добавьте chat.Id для остальных пользователей
                                                     5948245047,
                                                     1669823924,
                                                     1499207603,
                                                     1070635083,
                                                     1168649232,
                                                     5160414346,
                                                     738641967


                                                 };

                                        string messageText = "Привет, я ваш бот! Теперь у меня есть ваши чат идентификаторы. Настало время спам-атак!";

                                        foreach (long receiverChatId in receiverChatIds)
                                        {
                                            await botClient.SendTextMessageAsync(receiverChatId, messageText);
                                        }
                                        return;
                                    }
                                    
                                    if (message.Text == "/sendphoto" || message.Text == "мем" || message.Text == "Мемчик" || message.Text == "мемчик")
                                    {
                                        Random rnd = new Random();
                                        int memNumber = rnd.Next(0, 10);
                                        var linkPhoto = "";
                                        switch (memNumber)
                                        {
                                            case 0:
                                                linkPhoto = "https://cs13.pikabu.ru/post_img/big/2023/11/06/7/1699268870247742119.png";
                                                break;
                                            case 1:
                                                linkPhoto = "https://cs14.pikabu.ru/post_img/2023/11/06/9/1699285721120093151.jpg";
                                                break;
                                            case 2:
                                                linkPhoto = "https://cs14.pikabu.ru/post_img/2023/11/08/6/1699437225193596284.jpg";
                                                break;
                                            case 3:
                                                linkPhoto = "https://cs13.pikabu.ru/post_img/2023/11/08/6/1699437244110746943.jpg";
                                                break;
                                            case 4:
                                                linkPhoto = "https://cs13.pikabu.ru/post_img/2023/11/08/6/1699436521158580719.jpg";
                                                break;
                                            case 5:
                                                linkPhoto = "https://cs13.pikabu.ru/post_img/2023/11/07/10/1699374732148457417.webp";
                                                break;
                                            case 6:
                                                linkPhoto = "https://cs13.pikabu.ru/post_img/2023/11/07/12/1699388430168326538.webp";
                                                break;
                                            case 7:
                                                linkPhoto = "https://cs14.pikabu.ru/post_img/2023/11/07/7/169935373218156771.webp";
                                                break;
                                            case 8:
                                                linkPhoto = "https://cs14.pikabu.ru/post_img/2023/11/07/9/1699366842123162256.jpg";
                                                break;
                                            case 9:
                                                linkPhoto = "https://cs14.pikabu.ru/post_img/2023/11/07/11/1699381515169811193.jpg";
                                                break;
                                            case 10:
                                                linkPhoto = "https://cs13.pikabu.ru/post_img/2023/11/07/10/1699377109175352569.jpg";
                                                break;
                                            
                                        }
                                        InputFileUrl inputonlinefile = new(linkPhoto);
                                        await botClient.SendPhotoAsync(chat.Id, inputonlinefile);
                                        return;
                                           
                                       
                                    }




                                    if (message.Text == "/inline")
                                    {
                                        // Тут создаем нашу клавиатуру
                                        var inlineKeyboard = new InlineKeyboardMarkup(
                                            new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                                            {
                                        // Каждый новый массив - это дополнительные строки,
                                        // а каждая дополнительная строка (кнопка) в массиве - это добавление ряда

                                        new InlineKeyboardButton[] // тут создаем массив кнопок
                                        {
                                            InlineKeyboardButton.WithUrl("Это кнопка с самым важным сайтом для каждого из нас", "https://openai.com/"),
                                            InlineKeyboardButton.WithUrl("ПОсмотреть свои долги в Google Класс", "https://classroom.google.com/a/not-turned-in/all"),
                                        }
                                       
                                            });

                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Это inline клавиатура!",
                                            replyMarkup: inlineKeyboard); // Все клавиатуры передаются в параметр replyMarkup

                                        return;
                                    }

                                    if (message.Text == "/reply")
                                    {
                                        // Тут все аналогично Inline клавиатуре, только меняются классы
                                        // НО! Тут потребуется дополнительно указать один параметр, чтобы
                                        // клавиатура выглядела нормально, а не как абы что

                                        var replyKeyboard = new ReplyKeyboardMarkup(
                                            new List<KeyboardButton[]>()
                                            {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Привет!"),
                                            new KeyboardButton("Пока!"),
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Позвони мне!")
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Напиши моему соседу!")
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Мемчик")
                                        }
                                            })
                                        {
                                            // автоматическое изменение размера клавиатуры, если не стоит true,
                                            // тогда клавиатура растягивается чуть ли не до луны,
                                            // проверить можете сами
                                            ResizeKeyboard = true,
                                        };

                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Это reply клавиатура!",
                                            replyMarkup: replyKeyboard); // опять передаем клавиатуру в параметр replyMarkup

                                        return;
                                    }

                                    return;
                                }

                            // Добавил default , чтобы показать вам разницу типов Message
                            default:
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Используй только текст!");
                                    return;
                                }
                        }

                        return;  
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }



}
