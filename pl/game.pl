:- dynamic yes/1, no/1.

go :- nl,
      write('Piensa en un personaje de Marvel y tratare de adivinar quien es. ¿Has pensado en uno (si/no)?'),
      nl,
      read(Response),
      nl,
      (Response == si ; Response == s),
      write('Genial Ahora, responde si o no a las siguientes preguntas'),
      nl, nl,
      guess(Personaje),
      write('El personaje es '),
      write(Personaje),
      nl,
      undo.


/* hipótesis a ser probada */
guess(magneto)   :- magneto, !.
guess(loki) :- loki, !.
guess(encantadora) :- encantadora, !.
guess(craneo_rojo)   :- cabeza_roja, !.
guess(duende_verde) :- duende_verde, !.
guess(ciclope) :- ciclope, !.
guess(deadpool):- deadpool,!.
guess(tormenta) :- tormenta, !.
guess(daredevil) :- daredevil, !.
guess(capitan_america) :- capitan_america, !.
guess(doctor_strano) :- doctor_strano,!.
guess(viuda_negra) :- viuda_negra, !.
guess(bruja_escarlata) :-bruja_escarlata, !.
guess(thor) :- thor, !.
guess(odin) :- odin, !.
guess(lady_sif) :- lady_sif, !.
guess(spiderman) :- spiderman, !.
guess(hulk) :- hulk, !.
guess(iron_man) :- iron_man, !.
guess(desconocido).


es_heroe:- verify(es_heroe),!.
villano:- not(es_heroe),!.

controMagico :- verify(controla_magia),!.

humano:- (es_heroe ; villano),
    verify(es_humano),!.

asgardiano:- (es_heroe ; villano),
    not(humano),!.

vengador:- (humano ; asgardiano),
    verify(es_miembro_de_los_vengadores),!.

xmen:- humano,
    not(vengador),
    verify(es_miembro_de_los_xmen),!.

mutante:- humano,
    verify(es_mutante),!.

hydra:- humano,
    verify(es_miembro_de_hydra),!.
	
deadpool:- humano,
	mutante,
	verify(es_del_genero_masculino),
	verify(utiliza_catanas).
	
magneto :- humano,
    villano,
    mutante,!,
	verify(controla_metal).

cabeza_roja :- humano,
    villano,
    not(mutante),
    hydra,!.

duende_verde :- humano,
    villano,
    not(mutante),
    not(hydra),!,
	verify(utiliza_planeador).

loki :- asgardiano,
    villano,
    verify(es_del_genero_masculino),!,
	verify(su_hermano_es_Thor).

encantadora :- asgardiano,
    villano,
    verify(es_del_genero_femenino),!.

ciclope :- humano,
    es_heroe,
    xmen,
    verify(es_del_genero_masculino),!.
	

tormenta :- humano,
    es_heroe,
    xmen,
    verify(es_del_grnero_femenino),!.

daredevil :- humano,
    es_heroe,
    not(vengador),
    not(xmen),!.

iron_man :- humano,
    vengador,
    es_heroe,
    verify(es_del_genero_masculino),!,
    verify(es_multimillonario_y_genio).

capitan_america :- humano,
    es_heroe,
    vengador,
    verify(es_del_genero_masculino),!,
	verify(utiliza_escudo),!.

doctor_strano:- humano,
    es_heroe,
    vengador,
    controMagico.

bruja_escarlata :- humano,
    es_heroe,
    controMagico,
    vengador,
    verify(es_del_genero_femenino),!.
    
viuda_negra :- humano,
    es_heroe,
    vengador,
    not(controMagico),
    verify(es_del_genero_femenino),!.


thor :- asgardiano,
    es_heroe,
    vengador,!.

odin :- asgardiano,
    es_heroe,
    not(vengador),
    verify(es_del_genero_masculino),!.

lady_sif :- asgardiano,
    es_heroe,
    not(vengador),
    verify(es_del_genero_femenino),!.


spiderman :- humano,
    es_heroe,
    verify(es_del_genero_masculino),!,
    verify(tiene_poderes_aracnidos),!.

hulk :- humano,
    es_heroe,
    verify(tuvo_un_accidente_con_rayos_gama),!.


no(es_del_genero_femenino):- yes(es_del_genero_masculino).
yes(es_del_genero_femenino):- no(es_del_genero_masculino).



ask(Pregunta) :-
    write('¿El personaje '),
    write(Pregunta),
    write('? '),
    read(Response),
    nl,
    ( (Response == si ; Response == s)
      ->
      assert(yes(Pregunta)) ;
       assert(no(Pregunta)), fail).


/* Verificador de respuestas */
verify(S) :-
    (yes(S) -> true ; (no(S) -> fail ; ask(S))).

/* Deshacer afirmaciones */
undo :- retract(yes(_)),fail.
undo :- retract(no(_)),fail.

