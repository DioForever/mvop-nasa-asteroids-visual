# Nasa Asteroid Visual

## Obsah

1. [Úvod](#uvod)
  1.1 [Účel](#uvod-ucel)  
  1.2 [Konvence dokumentu](#uvod-konvence)  
  1.3 [Cílová skupina](#cilovka)  
  1.4 [Kontakty](#kontakt)  
2. [Popis](#popis)
  2.1 [Produkt](#popis-produkt)  
  2.2 [Funkce](#popis-funkce)  
  2.3 [Uživatelské skupiny](#popis-skupiny)  
  2.4 [Provozní prostředí](#popis-prov-prostredi)  
  2.5 [Uživatelské prostředí](#popis-uziv-prostredi)  
  2.7 [Předpoklady a závislosti](#popis-zavislosti)  
3. [Požadavky na rozhraní](#pozadavky)
  3.1 [Uživatelské rozhraní](#pozadavky-rozhrani)  
4. [Vlastnosti systému](#vlastnosti)
  4.1 [Zobrazení asteroidů](#vlastnosti-zobrazeni)  
  4.2 [Filtrování asteroidů](#vlastnosti-filtrovani)  
5. [Nefunkční požadavky](#nefunkcni)
  5.1 [Výkonnost](#nefunkcni-vykonnost)  
  5.2 [Bezpečnost](#nefunkcni-bezpecnost)  
  5.3 [Spolehlivost](#nefunkcni-spolehlivost)  
  5.4 [Projektová dokumentace](#nefunkcni-dokumentace)  

## Historie dokumentu

| Verze | Autor | Komentář |
|-------|-------|----------|
| 1   | Vasko Daniel | První verze dokumentu |

- ## 1. Úvod {#uvod}

  - ### 1.1 Účel {#uvod-ucel}

    - Zobrazit vesmírné tělesa ve 2d zobrazení z daného datumu.

  - ### 1.2 Konvence dokumentu {#uvod-konvence}

    - Exe soubor v jazyku C#.

  - ### 1.3 Cílová skupina {#cilovka}

    - Tento dokument je určený pro ty, kdo se chtějí vidět vesmírná tělěsa zachycená a poskytnutá od NASA API.

  - ### 1.4 Kontakty {#kontakt}

    - Mail: <vaskodaniel1@gmail.com>

- ## 2. Popis {#popis}

  - ### 2.1 Produkt {#popis-produkt}

    Produkt bude vyvářen v jazyku C# a bude spouštěn jako .exe soubor.

  - ### 2.2 Funkce {#popis-funkce}

    Funkce budou, **přidávání a ukládání** vlastního NASA API klíče pro přístup k API, po vybrání datumu **zobrazení asteroidů** zaznamenané v API.

  - ### 2.3 Uživatelské skupiny {#popis-skupiny}

    Bude pouze jedna uživatlská skupina a to uživatelé, kteří mají přístup ke všem funkcím aplikace.

  - ### 2.4 Provozní prostředí {#popis-prov-prostredi}

    Tato aplikace je cílená pouze na android zařízení.

  - ### 2.5 Uživatelské prostředí {#popis-uziv-prostredi}

    Input pro NASA API klíč. Datový input pro zadání dne, který je vyžadováno zobrazit a tlačítko, které potvrdí výběr a zapne zobrazení asteroidů.

  - ### 2.7 Předpoklady a závislosti {#popis-zavislosti}

    Předpoklad, že si uživatel vytvoří účet na <https://api.nasa.gov> a použije jeho vlastní API klíč.
    Malé využití CPU a paměti, vyžadováno připojení k wifi, jednoduché a rychlé používání.

- ## 3. Požadavky na rozhraní {#pozadavky}

  - ### 3.1 Uživatelské rozhraní {#pozadavky-rozhrani}

    Mobilní aplikace, kde na jedné stránce bude moct filtrovat a ihned zobrazit asteroidy.

- ## 4. Vlastnosti systému {#vlastnosti}

  - ### 4.1 Zobrazení asteroidů {#vlastnosti-zobrazeni}

    - Důležitost: **HIGH**
    - Zobrazení asteroidů s **intensitou barvy** podle jejich "absolute_magnitude_h" vlastnosti. **Červené ohraničení** pokud je asteroid potenciálně hazardní podle vlastnosti "is_potentially_hazardous_asteroid".

  - ### 4.2 Filtrování asteroidů {#vlastnosti-filtrovani}

    - Důležitost: **HIGH**
    - Filtrování asteroidů podle jejich vlastností jako je velikost, nebezpečé".

- ## 5. Nefunkční požadavky {#nefunkcni}

  - ### 5.1 Výkonnost {#nefunkcni-vykonnost}

    - Výkonnost této aplikace záleží na rychlosti odpovědi API, tudíž nelze specifikovat požadovanou rychlost aplikace.

  - ### 5.2 Bezpečnost {#nefunkcni-bezpecnost}

    - Vzhledem k tomu, že tato aplikace neobsahuje žádné citlivé informace až na NASA API klíč, není potřeba řešit.

  - ### 5.3 Spolehlivost {#nefunkcni-spolehlivost}

    - Fukčnost aplikace (zapnutí, vypnutí).
    - Zajistit správného zobrazení asteroidů.

  - ### 5.4 Projektová dokumentace {#nefunkcni-dokumentace}

    - Současný dokument.
