<p align="right">
  <a href="https://www.235u.net">
    <img src="ActinUranium.Web/wwwroot/img/logo.svg" alt="Actin Uranium logo" width="109">
  </a>
</p>

## State

### Monitoring

[![Azure DevOps builds](https://img.shields.io/azure-devops/build/235u/ActinUranium.Web/2?style=for-the-badge)](https://dev.azure.com/235u/ActinUranium.Web/_build?definitionId=2)
[![Website](https://img.shields.io/website?style=for-the-badge&url=https%3A%2F%2Fwww.235u.net)](https://www.235u.net)
[![Uptime Robot ratio (30 days)](https://img.shields.io/uptimerobot/ratio/m783489721-6dbd879caf5b391ffe19c142?style=for-the-badge)](https://uptimerobot.com/)
[![Mozilla HTTP Observatory Grade](https://img.shields.io/mozilla-observatory/grade/www.235u.net?publish&style=for-the-badge)](https://observatory.mozilla.org/analyze/www.235u.net)

### Other

![GitHub language count](https://img.shields.io/github/languages/count/235u/ActinUranium.Web?style=for-the-badge)
[![GitHub top language](https://img.shields.io/github/languages/top/235u/ActinUranium.Web?style=for-the-badge)](https://github.com/search?q=repo%3A235u%2FActinUranium.Web+language%3AC%23&type=Code&ref=advsearch&l=C%23)

## Observations

### Globalization and localization
Setting (via `Project Properties > Package > Assembly neutral language` or manually in `.csproj`)

```xml
<PropertyGroup>
  <NeutralLanguage>de</NeutralLanguage>
</PropertyGroup>
```

leads to strange (mixed) localization behaviour at runtime; localization works as expected while leaving this setting empty (or setting to `(None)` in the Properties GUI).

## Optimizations

- [ ] Refactor style sheets.
- [ ] Redraw logo, icons, and illustrations.
- [ ] Animate icons (s. [How SVG Line Animation Works](https://css-tricks.com/svg-line-animation-works))?
- [ ] Enhance responsiveness?
- [ ] Implement random color palette.