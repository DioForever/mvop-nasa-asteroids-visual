# Nasa Asteroid Visual

- ## 1. Úvod

  - ### 1.1 Účel

    - Zobrazit vesmírné tělesa ve 2d zobrazení z daného datumu.

  - ### 1.2 Konvence dokumentu

    - Exe soubor v jazyku C#.

  - ### 1.3 Cílová skupina

    - Tento dokument je určený pro ty, kdo se chtějí vidět vesmírná tělěsa zachycená a poskytnutá od NASA API.

  - ### 1.4 Kontakty

    - Mail: <vaskodaniel1@gmail.com>

- ## 2. Popis

  - ### 2.1 Produkt

    Produkt bude vyvářen v jazyku C# a bude spouštěn jako .exe soubor.

  - ### 2.2 Funkce

    Funkce budou, **přidávání a ukládání** vlastního NASA API klíče pro přístup k API, po vybrání datumu **zobrazení asteroidů** zaznamenané v API.

  - ### 2.3 Uživatelské skupiny

    Bude pouze jedna uživatlská skupina a to uživatelé, kteří mají přístup ke všem funkcím aplikace.

  - ### 2.4 Provozní prostředí

    Tato aplikace je cílená pouze na android zařízení.

  - ### 2.5 Uživatelské prostředí

    Input pro NASA API klíč. Datový input pro zadání dne, který je vyžadováno zobrazit a tlačítko, které potvrdí výběr a zapne zobrazení asteroidů.

  - ### 2.6 Omezení návrhu a implementace

    ?

  - ### 2.7 Předpoklady a závislosti

    Předpoklad, že si uživatel vytvoří účet na <https://api.nasa.gov> a použije jeho vlastní API klíč.
    Malé využití CPU a paměti, vyžadováno připojení k wifi, jednoduché a rychlé používání.

- ## 3. Požadavky na rozhraní

  - ### 3.1 Uživatelské rozhraní
  
       
       Desktopová aplikace, nebude tam víc věcí než nutno, základní operace a další operace přes rozkliknutí možné přidat.

- ## 4. Vlastnosti systému

  - ### 4.1 Zobrazení asteroidů

    - Důležitost: **HIGH**
    - Zobrazení asteroidů s **intensitou barvy** podle jejich "absolute_magnitude_h" vlastnosti. **Červené ohraničení** pokud je asteroid potenciálně hazardní podle vlastnosti "is_potentially_hazardous_asteroid".

  - ### 4.2 Základní

    - Důležitost: **HIGH**
    - Mazání výpočtů, zobrazení výsledků.

- ## 5. Nefunkční požadavky

  - ### 5.1 Výkonnost

    - ?

  - ### 5.2 Bezpečnost

    - ?

  - ### 5.3 Spolehlivost

    - Fukčnost aplikace (zapnutí, vypnutí), předejít crashnutí aplikace z výpočetních důvodů.
    - Zajistit správného zobrazení asteroidů.

  - ### 5.4 Projektová dokumentace

    - Současný dokument.

  - ### 5.5 Uživatelská dokumentace

    - s