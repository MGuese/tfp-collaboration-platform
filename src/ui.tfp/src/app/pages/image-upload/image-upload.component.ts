import { Component, inject } from '@angular/core';
import { ImageUploadService } from '../../services/image-upload.service';

@Component({
  selector: 'app-image-upload',
  imports: [],
  templateUrl: './image-upload.component.html',
  styleUrl: './image-upload.component.css'
})
export class ImageUploadComponent {
  imageUploadService: ImageUploadService = inject(ImageUploadService)
  selectedFiles: File[] = [];

  onFileSelected(event: any) {
    console.log(event)
    if (!event.target.files) {
      console.error("No files selected")
      return
    }
    this.selectedFiles = event.target.files;
  }

  submitImages() {
    if (this.selectedFiles.length <= 0) {
      console.error("No files selected")
      return
    }

    const formData = new FormData();
    for (let i = 0; i < this.selectedFiles.length; i++) {
      formData.append("images", this.selectedFiles[i]);
    }

    try {
      this.imageUploadService.uploadImage(formData)
    } catch (e) {
      console.error(e)
    }
  }

  removeImage(imageName: string) {
    const currentFiles = this.selectedFiles
    this.selectedFiles = []

    for (let i = 0; i < currentFiles.length; i++) {
      if (currentFiles[i].name !== imageName) {
        this.selectedFiles.push(currentFiles[i])
      }
    }
  }
}
