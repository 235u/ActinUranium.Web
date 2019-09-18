<p align="right">
  <a href="https://www.235u.net">
    <img src="ActinUranium.Web/wwwroot/img/logo.svg" alt="Actin Uranium logo" width="109">
  </a>
</p>

## Ideas

- Nachbau auch mit CMS (Umbraco / Orchard), Performance-Vergleich
- Time-Machine k�nnte f�r Referenzen (interne Entwicklung, Verweise zu externen Ressourcen) verwendet werden.
- Time-Machine-Funktionalit�t
  - Verlinkung zu bestimmten Versionen, statt Screenshots
  - Eine Multi-Tenancy-Funktionalit�t
- Business Lunch, Gesamtprojekt (Planung, Konstruktion etc.) als Schlagzeilen und Arbeiten
- Developer PC Build

## Observations

### Globalization and localization
Setting (via `Project Properties > Package > Assembly neutral language` or manually in `.csproj`)

```xml
<PropertyGroup>
  <NeutralLanguage>de</NeutralLanguage>
</PropertyGroup>
```

leads to strange (mixed) localization behaviour at runtime; localization works as expected while leaving this setting empty (or setting to `(None)` in the Properties GUI.

## Versioning

- 2019.01.25 22:39:11
- 2019.07.15.1730
- 2019.07

## Text-Bausteine

- Ich werde nicht nur Software entwickeln, sondern auch viel Software anwenden / einsetzen.

## Built with

In Form einer sch�nen Tabelle; eventuell als SVG

- <https://www.codingdojo.com/web-development-courses>
- <https://builtwith.com/>

## Referenzen

- Stack Overflow
- CSS Tricks
- Wikipedia
- Google Styles
- Pixabay
- Creative Market
- Font Awesome
- Microsoft
  - Design Guidelines
- Coding Guidelines
 - Google
  - Styles
  - Speed
 - MDN
 - W3C
 - Adobe

## Data Seeding

Nicht per Session - der Speicher w�rde explodieren.

