import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { MovieScreening } from '../../models/movie_screening.model';
import { Movie } from '../../models/movie.model';
import { Genre } from '../../models/genre.model';

import { MovieScreeningService } from '../../services/movie/movie-screening-service';
import { GenreService } from '../../services/genre/genre';

@Component({
  selector: 'app-homepage-component',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule
  ],
  templateUrl: './homepage-component.html',
  styleUrl: './homepage-component.css',
})
export class HomepageComponent implements OnInit {

  screenings: MovieScreening[] = [];

  genres: Genre[] = [];


  selectedGenreId: number | null = null;

  selectedDate: string | null = null;

  sortBy: 'chronologically' | 'alphabetically'
      = 'chronologically';



  groupedMovies:
    {movie: Movie, screenings: MovieScreening[]}[]
    = [];



  constructor(
    private movieScreeningService: MovieScreeningService,
    private genreService: GenreService
  ){}



  ngOnInit(): void {

    this.loadGenres();

    this.loadScreenings();

  }




  loadGenres(){

    this.genreService.getAll()
      .subscribe({

        next:(data)=>{

          this.genres = data;

        },

        error:(err)=>{

          console.error(err);

        }

      });

  }





  loadScreenings(){

    this.movieScreeningService
      .getUpcoming7Days(
        this.selectedGenreId,
        this.selectedDate,
        this.sortBy
      )
      .subscribe({

        next:(data:any)=>{

          console.log("SCREENINGS RESPONSE", data);

            const screenings =
              Array.isArray(data)
              ? data
              : data.items;
          this.screenings = screenings;

          this.groupByMovie(screenings);

        }, 
        error:(err)=>{
          console.error(err);
        }

      });

  }




  filterChanged(){

    this.loadScreenings();

  }





  groupByMovie(screenings: MovieScreening[]){


    const map = new Map<number,{movie:Movie,screenings:MovieScreening[]}>();


    screenings.forEach(s=>{

    if(!s.movie)
      return;


    if(!map.has(s.movieId)){

      map.set(
        s.movieId,
      {
        movie:s.movie,
        screenings:[]
      }
    );
  }


    map.get(s.movieId)!
    .screenings
    .push(s);


  });


    this.groupedMovies =
    Array.from(map.values());
  }





  get next7Days():Date[]{

    return Array.from(
      {length:7},
      (_,i)=>{

        const date = new Date();

        date.setDate(
          date.getDate()+i
        );

        return date;

      }
    );

  }





  getScreeningsForDay(
    movieScreenings:MovieScreening[],
    day:Date
  ){

    return movieScreenings.filter(s=>{

      const d =
        new Date(s.startTime);


      return d.toDateString()
        === day.toDateString();

    });

  }





  isPast(screening:MovieScreening){

    return new Date(screening.startTime)
      <
      new Date();

  }



}