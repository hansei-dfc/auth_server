using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using war_game.SMTP;
using war_game.Utility;

namespace war_game.Controllers {
    [ApiController]
    [Route("auth")]
    public class SignuPController : ControllerBase {
        private readonly ILogger<SignuPController> _logger;
        private readonly Database database;
        private readonly SmtpService smtp;

        // ��� ���Խ� | �ּ� 8 �� �� �ִ� 10 ��, �빮�� �ϳ� �̻�, �ҹ��� �ϳ�, ���� �ϳ� �� Ư�� ���� �ϳ� �̻�
        static readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,10}", RegexOptions.Compiled);
        static readonly Regex EmailRegex = new(@"^([\w._-])*[a-zA-Z0-9]+([\w._-])*([a-zA-Z0-9])+([\w._-])+@([a-zA-Z0-9]+.)+[a-zA-Z0-9]{2,8}$", RegexOptions.Compiled);

        public SignuPController(ILogger<SignuPController> logger, Database db, SmtpService smtp) {
            _logger = logger;
            database = db;
            this.smtp = smtp;
        }

        public class SignUp {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost(Name = "signup")]
        public async Task<dynamic> SignUpAsync(SignUp? post) {
            if (post == null) return ApiResult.Failure(0, "������ ���� ����");
            if (post.Email.IsNullOrWhiteSpace() || !EmailRegex.IsMatch(post.Email)) return ApiResult.Failure(1, "�̸��� �ƴ� ����");
            if (post.Password.IsNullOrWhiteSpace() || !PasswordRegex.IsMatch(post.Password)) return ApiResult.Failure(3, "��� ���� ���� ����");
            // �ߺ� Ȯ��
            if (await database.ExecuteNonQueryAsync($"select exists(select 1 from `users` where email=@E)", null, "E", post.Email) != -1)
                return ApiResult.Failure(3, "�ߺ� ����");

            smtp.SendVerifyMail(post.Email, "asdf");

            return ApiResult.Success();
        }

        async Task<ulong> CreateIdAsync() {
        RET:
            var id = RandomUtility.CreateId();
            if (await database.ExecuteNonQueryAsync($"select exists(select 1 from `users` where id=@ID)", null, "ID", id) != -1)
                goto RET;
            return id;
        }

        async Task<ulong> CreateEmailVerifyIdAsync() {
        RET:
            var id = RandomUtility.CreateId();
            if (await database.ExecuteNonQueryAsync($"select exists(select 1 from `users` where verifyMail=@ID)", null, "ID", id) != -1)
                goto RET;
            return id;
        }

        async Task<ulong?> CreateUserAsync(SignUp data, bool useVefEmail) {
            var id = await CreateIdAsync();
            var verId = useVefEmail ? await CreateEmailVerifyIdAsync() : 0;
            if (await database.ExecuteNonQueryAsync(
                $"insert into `users` (`id`, `email`, `password`, `createAt`, `verifyMail`) VALUES (@ID, @EMAIL, @PASSWORD, @CREATEAT, @VEM)", null,
                "ID", id, "EMAIL", data.Email, "PASSWORD", data.Password, "CREATEAT", DateTime.Now, "VEM", verId) != 0) return null;
            return id;
        }
    }
}