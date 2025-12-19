import { Component, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-image-upload',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './image-upload.component.html',
    styleUrl: './image-upload.component.css'
})
export class ImageUploadComponent {
    // الأحداث المرسلة للمكون الأب
    imageSelected = output<File>();

    // الحالة
    previewUrl = signal<string | null>(null);
    isDragging = signal(false);
    fileName = signal<string | null>(null);

    onDragOver(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
        this.isDragging.set(true);
    }

    onDragLeave(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
        this.isDragging.set(false);
    }

    onDrop(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
        this.isDragging.set(false);

        const files = event.dataTransfer?.files;
        if (files && files.length > 0) {
            this.handleFile(files[0]);
        }
    }

    onFileSelect(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.handleFile(input.files[0]);
        }
    }

    private handleFile(file: File): void {
        // التحقق من نوع الملف
        if (!file.type.startsWith('image/')) {
            alert('يرجى اختيار ملف صورة صالح');
            return;
        }

        // التحقق من الحجم (حد أقصى 10MB)
        if (file.size > 10 * 1024 * 1024) {
            alert('حجم الصورة يجب أن يكون أقل من 10 ميجابايت');
            return;
        }

        this.fileName.set(file.name);

        // إنشاء معاينة
        const reader = new FileReader();
        reader.onload = (e) => {
            this.previewUrl.set(e.target?.result as string);
        };
        reader.readAsDataURL(file);

        // إرسال الملف للمكون الأب
        this.imageSelected.emit(file);
    }

    clearImage(): void {
        this.previewUrl.set(null);
        this.fileName.set(null);
    }
}
