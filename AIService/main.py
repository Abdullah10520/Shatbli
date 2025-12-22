import google.genai as genai
from google.genai import types
from fastapi import FastAPI, File, UploadFile, Form
from fastapi.responses import Response
import os

app = FastAPI()

api_key = os.getenv("GEMINI_API_KEY")

if not api_key:
    raise RuntimeError("GEMINI_API_KEY environment variable is not set")

client = genai.Client(api_key=api_key)


@app.post("/roomCeramic")
async def generateRoomWithCeramicTile(roomImage: UploadFile = File(...), ceramicTileImage: UploadFile = File(...)):
    room_bytes = await roomImage.read()
    tile_bytes = await ceramicTileImage.read()

    prompt = (
        "You are a professional architectural renderer. Produce one PHOTOREALISTIC edit of IMAGE 1 (the apartment interior)."
        "\n\nStrict instructions:"
        "\n1) FLOOR: Replace the entire visible floor surface in IMAGE 1 with the ceramic tile pattern from IMAGE 2."
        " Remove all sand, dust, concrete, or debris before applying the tile."
        " The original floor must be completely replaced — ensure full coverage with no visible old material."
        "\n2) TILE APPEARANCE: Match the scale, orientation, grout spacing, and color tone exactly as seen in IMAGE 2."
        " The pattern and color of the tile are extremely IMPORTANT — replicate them with perfect accuracy."
        " Align the tiles to the apartment’s floor plane using realistic perspective projection (correct vanishing lines)."
        "\n4) Preserve all furniture, objects, shadows, reflections, and lighting direction from IMAGE 1."
        " Only modify the floor material and wall color — do not alter geometry, decor, or lighting."
        "\n5) Do not hallucinate or remove objects. Do not modify the structure, windows, or perspective."
        "\n6) The new floor should appear recently installed, clean, and professionally finished."
        " Include subtle reflections and realistic surface gloss typical of ceramic tiles."
        "\n7) Output exactly one high-resolution, photorealistic image (PNG only)."
        " Do not include text, borders, watermarks, or artistic filters."
        "\n\nRendering quality goals: physically-based realism, accurate light interaction, smooth surfaces,"
        " consistent perspective alignment, and no visible seams or distortions."
    )

    response = client.models.generate_content(
        model="gemini-2.5-flash-image",
        contents=[
            prompt,
            types.Part(
                inline_data=types.Blob(
                mime_type="image/png",
                data=room_bytes
                )
            ),
            types.Part(
                inline_data=types.Blob(
                mime_type="image/png",
                data=tile_bytes
                )
            )
        ],
        config={
            "temperature": 0.4,
            "top_p": 0.75,
            "max_output_tokens": 8192
        }
    )

    for part in response.candidates[0].content.parts:
        if part.inline_data is not None:
            return Response(
                content=part.inline_data.data,
                media_type=part.inline_data.mime_type
            )


@app.post("/roomWall")
async def generateRoomWithWallPainting(roomImage: UploadFile = File(...), wallPaintingImage: UploadFile = File(...)):
    room_bytes = await roomImage.read()
    wall_bytes = await wallPaintingImage.read()
    prompt = (
        "You are a professional architectural renderer and interior visualization expert. "
        "Produce one PHOTOREALISTIC edit of IMAGE 1 (the apartment interior)."
        "\n\nStrict instructions:"
        "\n1) WALLS & CEILING: Repaint ALL visible wall and ceiling surfaces in IMAGE 1 "
        "using the color and finish from IMAGE 2. "
        "Before painting, CLEAN and RESTORE the wall surfaces — remove all dust, stains, concrete patches, "
        "scratches, cracks, or unfinished textures so the surface becomes perfectly smooth and ready for painting."
        "\n2) COLOR MATCHING: Match the color, tone, and texture of the paint from IMAGE 2 with perfect accuracy. "
        "Apply the paint evenly and uniformly across all visible walls and the ceiling. "
        "Do not leave any unpainted or partially covered areas."
        "\n3) LIGHTING & REFLECTIONS: Maintain the natural lighting, shadows, and reflections from IMAGE 1. "
        "Ensure the new paint color reacts correctly to the light — brighter near light sources, softer in shadowed areas."
        "\n4) OBJECT PRESERVATION: Keep all furniture, floor materials, and decor unchanged. "
        "Only modify the wall and ceiling color. Do not remove or alter any objects."
        "\n5) STRUCTURE: Keep the wall geometry, edges, and corners exactly as in IMAGE 1. "
        "Do not add patterns, decorations, or new wall textures."
        "\n6) FINISH QUALITY: The painted surfaces should appear clean, fresh, and recently completed — "
        "smooth, consistent, and realistic with subtle gloss or matte reflection as seen in IMAGE 2."
        "\n7) Output exactly one high-resolution photorealistic image (PNG only). "
        "Do not include text, borders, or stylized filters."
        "\n\nRendering goals: physically-based realism, perfect wall cleanliness, smooth finish, "
        "accurate paint tone, and consistent light behavior."
    )
    response = client.models.generate_content(
        model="gemini-2.5-flash-image",
        contents=[
            prompt,
            types.Part(
                inline_data=types.Blob(
                mime_type="image/png",
                data=room_bytes
                )
            ),
            types.Part(
                inline_data=types.Blob(
                mime_type="image/png",
                data=wall_bytes
                )
            )
        ],
        config={
            "temperature": 0.4,
            "top_p": 0.75,
            "max_output_tokens": 8192
        }
    )

    for part in response.candidates[0].content.parts:
        if part.inline_data is not None:
            return Response(
                content=part.inline_data.data,
                media_type=part.inline_data.mime_type
            )

@app.post("/roomCeramicWithWallColor")
async def generateRoomWithCeramicAndWallColor(
    roomImage: UploadFile = File(...),
    ceramicTileImage: UploadFile = File(...),
    wall_color_hex: str = Form(...)
):
    # Read images
    room_bytes = await roomImage.read()
    tile_bytes = await ceramicTileImage.read()

    # Prompt with HEX color
    prompt = (
        "You are a professional architectural renderer. Produce one PHOTOREALISTIC edit of IMAGE 1 (the apartment interior)."
        "\n\nStrict instructions:"

        "\n1) FLOOR: Replace the entire visible floor surface in IMAGE 1 with the ceramic tile pattern from IMAGE 2."
        " Remove all sand, dust, concrete, or debris before applying the tile."
        " The original floor must be completely replaced — ensure full coverage with no visible old material."

        "\n2) WALL COLOR (WALLS ONLY): Repaint ALL visible WALL surfaces using the EXACT solid color specified by this HEX code: "
        f"{wall_color_hex}. "
        "The wall color must match this HEX code precisely with no variation, tint, or artistic interpretation."

        "\nIMPORTANT: DO NOT paint, recolor, or modify the CEILING in any way."
        " The ceiling must remain EXACTLY as it appears in IMAGE 1, including its original color, texture, lighting, and material."
        " Ceiling surfaces are STRICTLY excluded from repainting."

        "\n3) TILE APPEARANCE: Match the scale, orientation, grout spacing, and color tone exactly as seen in IMAGE 2."
        " The pattern and color of the tile are extremely IMPORTANT — replicate them with perfect accuracy."
        " Align the tiles to the apartment’s floor plane using realistic perspective projection (correct vanishing lines)."

        "\n4) SURFACE PREPARATION (WALLS ONLY): Before painting, CLEAN and RESTORE wall surfaces — remove dust, stains, cracks,"
        " concrete patches, or unfinished textures so the walls appear smooth and professionally painted."
        " Do NOT clean, modify, or alter the ceiling."

        "\n5) PRESERVATION: Preserve all furniture, objects, shadows, reflections, and lighting direction from IMAGE 1."
        " Only modify the FLOOR material and WALL color — do not alter geometry, decor, lighting, or ceiling surfaces."

        "\n6) CONSTRAINTS: Do not hallucinate or remove objects."
        " Do not modify the structure, windows, ceiling, or perspective."

        "\n7) FINISH QUALITY: The painted walls should appear clean, uniform, and recently completed,"
        " with realistic light interaction (brighter near light sources, softer in shadows)."

        "\n8) OUTPUT: Output exactly ONE high-resolution, photorealistic image (PNG only)."
        " Do not include text, borders, watermarks, or artistic filters."

        "\n\nRendering quality goals: physically-based realism, accurate light interaction,"
        " smooth surfaces, consistent perspective alignment, and no visible seams or distortions."
    )

    # Call Gemini
    response = client.models.generate_content(
        model="gemini-2.5-flash-image",
        contents=[
            prompt,
            types.Part(
                inline_data=types.Blob(
                    mime_type="image/png",
                    data=room_bytes
                )
            ),
            types.Part(
                inline_data=types.Blob(
                    mime_type="image/png",
                    data=tile_bytes
                )
            )
        ],
        config={
            "temperature": 0.4,
            "top_p": 0.75,
            "max_output_tokens": 8192
        }
    )

    # Return generated image
    for part in response.candidates[0].content.parts:
        if part.inline_data is not None:
            return Response(
                content=part.inline_data.data,
                media_type=part.inline_data.mime_type
            )