Kullanıcılar index sayfasıyla başlıyorlar. Daha önce bir hesap oluşturulmuşsa login olmaları beklenmektedir.
Daha önce hesap oluşturulmadıysa da link üzerinden registration sayfasına yönlendirilirler.
Burada gerekli bilgiler girildikten sonra, girmiş oldukları email hesabına validation code (6 haneli, rastgele) içeren bir email atılır. Bu kod aynı zamanda veritabanına da kaydedilir.
Register sayfasına yönlendirilen kullanıcıdan buradaki alana verification code girmesi beklenir.
Kod doğru girildiyse HomePage sayfası açılır.


Proje ASP.NET MVC ile olulturuldu.
Database işlemleri için Entity Framework kullanıldı ve CodeFirst yaklaşımıyla database oluşturuldu. (Admin seed dataile eklendi.)
