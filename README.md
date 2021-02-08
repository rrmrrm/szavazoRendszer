# szavazoRendszer
ez nem verziókezeléssel készült, csak publikussá akartam tenni.
Készítsünk egy online anonim szavazó rendszert. 
A kliens-szerver webalkalmazáson keresztül felhasználók meghatározott körének kérhetjük ki a véleményéta kérdés és a válasz opciók megadásával. 
A szavazás titkos, amelyet a programnak garantálnia kell (külön tárolandó a szavazatukat gyakorló felhasználók és a leadott szavazat).

A webes felületen az aktív kérdésekben lehet szavazni, valamint megtekinthetőek a korábbi szavazások eredményei.

    • A felhasználók email cím és jelszó megadásával regisztrálhatnak, valamint jelentkezhetnek be. A portál további funkciói csak bejelentkezést követően érhetőek el.
    • Bejelentkezést követően a főoldalon megjelenik az aktív szavazások listája amelyek az adott felhasználóhoz hozzá vannak rendelve. Aktív az a szavazás, amely már elkezdődött, de még nem fejeződött be és a felhasználó még nem szavazott. A szavazásokat a befejező dátumuk szerint növekvő sorrendben kell listázni a kérdés szövegének valamint a kezdő és befejező időpontnak a feltüntetésével.
    • Egy aktív szavazás kiválasztásával a weboldal jelenítse meg a kérdést és a válasz opciókat. Utóbbiak közül pontosan egyet kiválasztva lehet a szavazatot érvényesen leadni.
    • A bejelentkezett felhasználók egy másik oldalon kilistázhatják a már lezárult, hozzájuk rendelt szavazásokat. Lezárultnak tekintendő az a szavazás, amelynek befejező időpontja elmúlt, vagy ha az összes hozzá rendelt felhasználó szavazott már. A szavazások listáját lehessen szűrni a kérdés szövegének részlete vagy időintervallum alapján.
    • Egy lezárult szavazás kiválasztásával a weboldal jelenítse meg annak eredményét:
        ◦ szavazó résztvevők számaés százalékos értéke;
        ◦ válasz opciónként a szavazatok száma és százalékos értéke 
