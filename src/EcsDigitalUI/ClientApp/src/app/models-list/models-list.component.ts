import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-models-list',
  templateUrl: './models-list.component.html'
})
export class ModelsDataComponent {
  public makers: IMaker[];
  public models: IModel[];
  public newModel: IModel;
  public newModelExists: boolean;
  public http: HttpClient;
  constructor(http: HttpClient) {
    this.http = http;
    this.refreshMakers();
    this.refreshModels();
    this.resetNewModel();
  }

  private resetNewModel() {
    this.newModel = { relatedWords: null, makerName: null, name: null, makerId: 0, id: 0 };
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

  public async addNewModel() {
    const newModel = this.newModel;
    (await this.http.post<IModel>('http://localhost:58599/api/' + 'model', newModel))
      .subscribe({
        next: async () => {
          this.resetNewModel();
          while (!this.doesModelExists(newModel)) {
            await this.refreshModels();
            await new Promise(r => setTimeout(r, 800));
            this.validateModelExists();
          }
        },
        error: error => console.error(error)
      });
  }

  public validateModelExists() {
    if (this.doesNewModelExists()) {
      this.newModelExists = true;
    } else {
      this.newModelExists = false;
    }
  }

  doesNewModelExists() {
    if (this.newModel.name == null || this.newModel.name === "" || this.newModel.makerId === 0)
      return false;
    return !!this.models.find(p => p.name === this.newModel.name && p.makerId === this.newModel.makerId);
  }

  doesModelExists(model: IModel) {
    return !!this.models.find(p => p.name === model.name && p.makerId === model.makerId);
  }

}

interface IMaker {
  id: number;
  name: string;
}

interface IModel {
  id: number;
  makerId: number;
  name: string;
  makerName: string;
  relatedWords: string;
}
