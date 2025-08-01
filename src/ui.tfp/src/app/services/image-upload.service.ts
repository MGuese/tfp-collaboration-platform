import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageUploadService {

  constructor() { }

  uploadImage(formData: FormData) {
    console.log("Mock Upload")
    console.log(formData.getAll("images"))
  }
}
