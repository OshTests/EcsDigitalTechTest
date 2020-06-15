import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { cssColors } from "./cssColors";

@Component({
  selector: 'app-cars-list',
  templateUrl: './cars-list.component.html'
})
export class CarsDataComponent {
  public makers: IMaker[];
  public models: IModel[];
  public makersModels: IModel[];
  public colours: string[];
  public newCar: INewCar;
  public newCarExists: boolean;
  public yearInRange: boolean;
  public minYearRange: number;
  public maxYearRange: number;
  public http: HttpClient;
  public cars: ICar[];
  public makerId: number;

  constructor(http: HttpClient, @Inject('BASE_URL') apiUrl: string) {
    this.http = http;
    this.refreshMakers();
    this.refreshModels();
    this.resetNewCar();
    this.colours = cssColors;
    this.refreshCars();
    this.minYearRange = 1900;
    this.maxYearRange = new Date().getUTCFullYear() + 1;
    this.validateModelExists();
    this.validateYearInRangeExists();
  }

  private resetNewCar() {
    this.newCar = { colour: null, modelId: 0, year: 0 };
    this.makerId = 0;
    this.validateYearInRangeExists();
  }

  private updateModels() {
    this.makersModels = this.models.filter(model => model.makerId === this.makerId);
  }

  private refreshCars() {
    this.http.get<ICar[]>('http://localhost:58599/api/' + 'car').subscribe(result => {
      this.cars = result;
    }, error => console.error(error));
  }

  private refreshMakers() {
    this.http.get<IMaker[]>('http://localhost:58599/api/' + 'maker').subscribe(result => {
      this.makers = result;
    }, error => console.error(error));
  }

  private async refreshModels() {
    this.http.get<IModel[]>('http://localhost:58599/api/' + 'model').subscribe(result => {
      this.models = result;
    }, error => console.error(error));
  }

  public async addNewCar() {
    const newCar = this.newCar;
    (await this.http.post<INewCar>('http://localhost:58599/api/' + 'car', newCar))
      .subscribe({
        next: async () => {
          this.resetNewCar();
          while (!this.doesCarExists(newCar)) {
            this.refreshCars();
            await new Promise(r => setTimeout(r, 800));
            this.validateModelExists();
          }
        },
        error: error => console.error(error)
      });
  }

  public validateYearInRangeExists() {
    this.yearInRange = this.newCar.year === 0 || this.newCar.year > this.minYearRange && this.newCar.year < this.maxYearRange;
  }

  public validateModelExists() {
    if (this.doesNewCarExists()) {
      this.newCarExists = true;
    } else {
      this.newCarExists = false;
    }
  }

  doesNewCarExists() {
    if (this.newCar.colour == null || this.newCar.modelId === 0 || this.newCar.year === 0)
      return false;
    return this.doesCarExists(this.newCar);
  }

  doesCarExists(model: INewCar) {
    return !!this.cars.find(p =>
      p.modelId === model.modelId
      && p.colour === model.colour
      && p.year === model.year);
  }

}

interface ICar {
  id: number;
  modelId: number;
  model: string;
  maker: string;
  relatedWords: string;
  year: number;
  colour: string;
}

interface INewCar {
  modelId: number;
  year: number;
  colour: string;
}

interface IMaker {
  id: number;
  name: string;
}

interface IModel {
  id: number;
  name: string;
  makerId: number;
}
