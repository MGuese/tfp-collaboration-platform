import { Routes } from '@angular/router';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';
import { ImageUploadComponent } from './pages/image-upload/image-upload.component';

export const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'image-upload', component: ImageUploadComponent }
];
