import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class WeatherForecastService {
  private readonly endpoint:string = environment.API + "weatherforecast";

  constructor(private readonly httpClient:HttpClient) { }

  public getWeatherData(): Observable<any>{
    return this.httpClient.get<any>(this.endpoint);
  }
}
