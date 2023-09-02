# OfferChecker: Sledování Cen na Zbozi.cz s Notifikacemi přes Telegram

## Úvod
OfferChecker je nástroj, který umožňuje sledovat nejnižší ceny zboží na agregátoru Zbozi.cz. Na základě konfiguračních souborů můžete specifikovat, jaké produkty chcete sledovat, a jaké jsou vaše preference (např. minimální rating obchodu, počet recenzí atd.). Pokud se nejnižší cena změní, aplikace vás o tom okamžitě informuje pomocí Telegram kanálu.

## Instalace

1. Naklonujte tento repositář.
2. Nainstalujte všechny potřebné závislosti.

## Konfigurační soubory

### A) Appsettings.json
Nastavení pro Telegram bot a další globální nastavení.
```json
{
  "TelegramSettings": {
    "BotID": "<BOT_ID>",
    "ChatID": "<CHAT_ID>"
  }
}
```

### B) Config.json
Soubor, ve kterém definujete, které produkty chcete sledovat.
```json
[
  {
    "Name": "Samsung Galaxy S23 Ultra",
    "OnlyCertified": false,
    "RecensionCountLimit": 1000,
    "RatingLimit": 92,
    "URL": "https://www.zbozi.cz/vyrobek/samsung-galaxy-s23-ultra/?varianta=8-256-gb-phantom-black"
  },
  // další produkty
]
```
**Poznámka:** Minimální rating obchodu a minimální počet recenzí jsou volitelné parametry.

## Jak vytvořit Telegram bota

1. Otevřete Telegram a vyhledejte "BotFather".
2. Po spuštění BotFathera, použijte příkaz `/newbot` a postupujte podle instrukcí pro vytvoření nového bota.
3. Po úspěšném vytvoření bota získáte token (BotID), který vložte do `Appsettings.json`.

## Jak zjistit ID telegram kanálu

1. Přidejte bota do vašeho Telegram kanálu.
2. Otevřete API volání s URL: `https://api.telegram.org/bot<YourBOTToken>/getUpdates`.
3. V odpovědi najdete pole `chat` a v něm `id`, které je ID vašeho kanálu. Toto ID vložte do `Appsettings.json`.

## Spuštění
Po nakonfigurování všech potřebných souborů můžete aplikaci spustit.

## Licence
MIT

Pro další informace a dotazy se obracejte na vývojáře.
