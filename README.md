# 222-Sevcik-Michael

Repozitář obsahuje soubory související s ročníkovým projektem a bakalářskou prací na téma tvorba podnikového online reporting nástroje s důrazem na jednoduchost nasazení u středních až větších firem zabývajících se výrobou.

## Přiblížení projektu

Výše zmíněné firmy již většinou používají nějaký ERP systém, ale často nevyužívají plného potenciálu dat v něm se skrývajících. Zde vyvíjený BI nástroj si klade za cíl poskytnout generický pohled (datový model) na tyto data a pomocí něj umožnit velmi rychlou vizualizaci různých výrobních a ekonomických metrik, mimo to nástroj provede základní kontrolu importovaných dat a upozorní na výskyt některých častých druhů chyb.

## Tématické okruhy související s vývojem tohoto projektu

Níže je základní přehled témat, s kterými jsem se setkal při práci na tomto projektu, či se jim v brzké budoucnosti budu věnovat:

- Srovnání nástrojů pro vizualizaci TODO: Můžu zůžit výběr na Open source?
    - Vybrán metabase
- Srovnání možností importu dat s důrazem na jednoduchost
    - ETL nástroje
    - Vlastní mapper a ETL infrastruktura
    - Pravděpodobně vlastní řešení s ohledeme na jednoduchost použití. 
- Generický datový medel výrobce
    - Datové modely zákazníků
    - Generalizace
- Vlastní mapper
    - Knihovny pro mapování
    - Implementace
- Kontrola dat - spíše implementace
- Generické nástěnky
- Deploy BI nástroje v Kubernets

## Obsah repozitáře

Tento repozitář zatím obsahuje pouze:

- toto readme
- a [log](log.txt) využívaný jako prostor pro uchovávání různých poznámek a odkazů souvisejících s vývojem. Obsah není struktovaný, ani příliš okomentovaný, slouží pouze pro interní organizaci práce.